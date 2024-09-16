using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanRuouVang.Models;

namespace WebsiteBanRuouVang.Controllers
{
    public class BrandsController : Controller
    {
        private dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();

        // GET: Brands
        public ActionResult Index(string sortOrder = "")
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                if (roleCookie.Value == "admin")
                {
                    dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                    List<Brands> brand = db.Brands.ToList();
                    switch (sortOrder)
                    {
                        case "Name":
                            brand = brand.OrderBy(row => row.Name).ToList();
                            break;
                        case "BrandId":
                            brand = brand.OrderBy(row => row.BrandId).ToList();
                            break;
                        default:
                            break;
                    }
                    return View(brand);
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
                    ViewBag.Brand = db.Brands.ToList();
                    return View();
                }
            }
            return RedirectToAction("Login", "Users");
        }
        [HttpPost]
        public ActionResult Create(Brands b)
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            db.Brands.Add(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                if (roleCookie.Value == "admin")
                {
                    dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                    Brands b = db.Brands.Where(row => row.BrandId == id).FirstOrDefault();
                    return View(b);
                }
            }
            return RedirectToAction("Login", "Users");

        }
        [HttpPost]
        public ActionResult Delete(int id, Brands e)
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                if (roleCookie.Value == "admin")
                {
                    dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                    Brands brand = db.Brands.Where(row => row.BrandId == id).FirstOrDefault();

                    db.Brands.Remove(brand);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Login", "User");

        }
        public ActionResult Edit(int id)
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                if (roleCookie.Value == "admin")
                {
                    dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                    Brands brand = db.Brands.Where(row => row.BrandId == id).FirstOrDefault();
                    ViewBag.Brand = db.Brands.ToList();
                    return View(brand);
                }
            }
            return RedirectToAction("Login", "Users");

        }

        [HttpPost]
        public ActionResult Edit(int id, Brands e)
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            ViewBag.Brand = db.Brands.ToList();
            Brands brand = db.Brands.Where(row => row.BrandId == id).FirstOrDefault();

            //Update
            brand.Name = e.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
