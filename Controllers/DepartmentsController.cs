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
    public class DepartmentsController : Controller
    {
        private dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();

        // GET: Departments
        public ActionResult Index(string sortOrder = "")
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                List<Departments> departments = db.Departments.ToList();
                switch (sortOrder)
                {
                    case "Name":
                        departments = departments.OrderBy(row => row.Name).ToList();
                        break;
                    case "DepId":
                        departments = departments.OrderBy(row => row.DepId).ToList();
                        break;
                    default:
                        break;
                }
                return View(departments);
            }
            return RedirectToAction("Login", "Users");
        }

        public ActionResult Create()
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            ViewBag.Departments = db.Departments.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Departments dep)
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                db.Departments.Add(dep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Users");

        }
        public ActionResult Delete(int id)
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                Departments dep = db.Departments.Where(row => row.DepId == id).FirstOrDefault();
                return View(dep);
            }
            return RedirectToAction("Login", "Users");

        }
        [HttpPost]
        public ActionResult Delete(int id, Products e)
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            Departments dep = db.Departments.Where(row => row.DepId == e.Id).FirstOrDefault();
            db.Departments.Remove(dep);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var roleCookie = Request.Cookies["role"];
            if (roleCookie != null)
            {
                dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                Departments dep = db.Departments.Where(row => row.DepId == id).FirstOrDefault();
                return View(dep);
            }
            return RedirectToAction("Login", "Users");
        }

        [HttpPost]
        public ActionResult Edit(int id, Departments e)
        {
            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            Departments dep = db.Departments.Where(row => row.DepId == id).FirstOrDefault();

            //Update
            dep.Name = e.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
