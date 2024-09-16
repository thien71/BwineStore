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
    public class CartsController : Controller
    {
        private dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();

        // GET: Carts
        public ActionResult Index()
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                List<Carts> carts = db.Carts.ToList();
                return View(carts);
            }
            return RedirectToAction("Login", "Users");
        }

        public ActionResult Add(int id = 0)
        {
            if (id > 0)
            {
                dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                Carts cartItem = db.Carts.Where(c => c.Id == id).FirstOrDefault();
                if (cartItem != null)
                {
                    cartItem.Quantity += 1;
                }
                else
                {
                    Carts cart = new Carts();
                    cart.Id = id;
                    cart.Quantity = 1;
                    db.Carts.Add(cart);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult UpdateQuantity(int quan = 0, int proid = 0)
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            if (quan > 0)
            {
                Carts cartItem = db.Carts.Where(c => c.Id == proid).FirstOrDefault();
                if (cartItem != null)
                {
                    cartItem.Quantity = quan;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id = 0)
        {
            if (id > 0)
            {
                dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                Carts cartItem = db.Carts.Where(c => c.CartId == id).FirstOrDefault();
                if (cartItem != null)
                {
                    db.Carts.Remove(cartItem);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            using (dbQuanLiRuouEntities db = new dbQuanLiRuouEntities())
            {
                try
                {
                    List<Carts> allCartItems = db.Carts.ToList();
                    db.Carts.RemoveRange(allCartItems);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi xoá tất cả các mục: " + ex.Message);
                    return View();
                }
            }
        }
    }
}
