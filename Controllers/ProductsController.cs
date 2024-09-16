using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanRuouVang.Models;

namespace WebsiteBanRuouVang.Controllers
{
    public class ProductsController : Controller
    {
        private dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();

        // GET: Products
        public ActionResult Index(int categoryId = 0, string search = "", int page = 1, string sortBy = "", string sortOrder = "")
        {
            IQueryable<Products> query = db.Products;
            // Filter by category ID
            if (categoryId != 0)
            {
                query = query.Where(e => e.DepId == categoryId);
            }
            // Filter by search keyword
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.ToLower().Contains(search.ToLower()));
            }
            // Sắp xếp theo giá
            if (sortBy == "Price")
            {
                if (sortOrder == "desc")
                {
                    query = query.OrderByDescending(e => e.Price);
                }
                else
                {
                    query = query.OrderBy(e => e.Price);
                }
            }
            // Sắp xếp theo tên
            else if (sortBy == "Name")
            {
                if (sortOrder == "desc")
                {
                    query = query.OrderByDescending(e => e.Name);
                }
                else
                {
                    query = query.OrderBy(e => e.Name);
                }
            }
            List<Products> employees = query.ToList();
            ViewBag.Search = search;

            int NoOfRecordPerPage = 6;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(employees.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.CurrentDeptID = categoryId;
            employees = employees.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            ViewBag.DanhMuc = db.Departments.ToList();
            ViewBag.SanPham = employees;

            // Get the selected category name
            string selectedCategory = "";
            if (categoryId != 0)
            {
                Departments category = db.Departments.FirstOrDefault(d => d.DepId == categoryId);
                if (category != null)
                {
                    selectedCategory = category.Name;
                }
            }
            ViewBag.SelectedCategory = selectedCategory;

            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;

            return View(employees);
        }
        public ActionResult PhuKien()
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            List<Products> products = db.Products.ToList();
            return View(products);
        }

        public ActionResult Details(int id)
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            Products products = db.Products.Where(row => row.Id == id).FirstOrDefault();
            ViewBag.Product = products;
            Departments dep = db.Departments.Where(cat => cat.DepId == products.DepId).FirstOrDefault();
            ViewBag.dep = dep;
            Brands brand = db.Brands.Where(cat => cat.BrandId == products.BrandId).FirstOrDefault();
            ViewBag.brand = brand;
            return View(products);
        }
        public ActionResult About(string sortOrder = "")
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                if (roleCookie.Value == "admin")
                {
                    dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                    List<Products> products = db.Products.ToList();
                    switch (sortOrder)
                    {
                        case "Name":
                            products = products.OrderBy(row => row.Name).ToList();
                            break;
                        case "DepId":
                            products = products.OrderBy(row => row.DepId).ToList();
                            break;
                        case "Id":
                            products = products.OrderBy(row => row.Id).ToList();
                            break;
                        case "Price":
                            products = products.OrderBy(row => row.Price).ToList();
                            break;
                        case "BrandId":
                            products = products.OrderBy(row => row.BrandId).ToList();
                            break;

                        default:
                            break;
                    }
                    return View(products);
                }
            }
            return RedirectToAction("Login", "Users");
        }

        public ActionResult Create()
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                if (roleCookie.Value == "admin")
                {
                    dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                    ViewBag.Departments = db.Departments.ToList();
                    ViewBag.Brands = db.Brands.ToList();
                    return View();
                }
            }
            return RedirectToAction("Login", "Users");

        }
        [HttpPost]
        public ActionResult Create(Products product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                ViewBag.Departments = db.Departments.ToList();
                ViewBag.Brands = db.Brands.ToList();

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    if (imageFile.ContentLength > 2000000)
                    {
                        ModelState.AddModelError("Image", "Kích thước file không được lớn hơn 2MB.");
                        return View();
                    }

                    var allowExs = new[] { ".jpg", ".png" };
                    var fileEx = Path.GetExtension(imageFile.FileName).ToLower();
                    if (!allowExs.Contains(fileEx))
                    {
                        ModelState.AddModelError("Image", "Phần mở rộng file không hỗ trợ.");
                        return View();
                    }

                    product.Image = "";
                    db.Products.Add(product);
                    db.SaveChanges();

                    Products pro = db.Products.ToList().Last();

                    var fileName = pro.Id.ToString() + fileEx;
                    var path = Path.Combine(Server.MapPath("~/Image"), fileName);
                    imageFile.SaveAs(path);

                    pro.Image = fileName;
                    db.SaveChanges();
                    return RedirectToAction("About");
                }
                else
                {
                    product.Image = "";
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("About");
                }

            }
            else
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                if (roleCookie.Value == "admin")
                {
                    dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                    Products emp = db.Products.Where(row => row.Id == id).FirstOrDefault();
                    return View(emp);
                }
            }
            return RedirectToAction("Login", "Users");

        }
        [HttpPost]
        public ActionResult Delete(int id, Product e)
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            Products employee = db.Products.Where(row => row.Id == e.Id).FirstOrDefault();
            db.Products.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("About");
        }
        public ActionResult Edit(int id)
        {
            var roleCookie = Request.Cookies["auth"];
            if (roleCookie != null)
            {
                if (roleCookie.Value == "admin")
                {
                    dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                    Products employee = db.Products.Where(row => row.Id == id).FirstOrDefault();
                    ViewBag.Departments = db.Departments.ToList();
                    ViewBag.Brands = db.Brands.ToList();
                    return View(employee);
                }
            }
            return RedirectToAction("Login", "Users");

        }

        [HttpPost]
        public ActionResult Edit(int id, Product e, HttpPostedFileBase Image)
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            ViewBag.Departments = db.Departments.ToList();
            Products product = db.Products.Where(row => row.Id == e.Id).FirstOrDefault();

            product.Name = e.Name;
            product.Price = e.Price;
            product.Detail = e.Detail;
            product.DepId = e.DepId;
            product.BrandId = e.BrandId;

            if (Image != null && Image.ContentLength > 0)
            {
                if (Image.ContentLength > 2000000)
                {
                    ModelState.AddModelError("Image", "Kích thước file không được lớn hơn 2MB.");
                    return View();
                }

                var allowExs = new[] { ".jpg", ".png" };
                var fileEx = Path.GetExtension(Image.FileName).ToLower();
                if (!allowExs.Contains(fileEx))
                {
                    ModelState.AddModelError("Image", "Phần mở rộng file không hỗ trợ.");
                    return View();
                }

                var fileName = product.Id.ToString() + fileEx;
                var path = Path.Combine(Server.MapPath("~/Image"), fileName);
                Image.SaveAs(path);

                product.Image = fileName;

                db.SaveChanges();
                return RedirectToAction("About");
            }
            else
            {
                product.Image = e.Image;
                db.SaveChanges();
                return RedirectToAction("About");
            }
        }
    }
}
