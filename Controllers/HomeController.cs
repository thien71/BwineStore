using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanRuouVang.Models;

namespace WebsiteBanRuouVang.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            List<Products> products = db.Products.ToList();
            return View(products);
        }
    }
}