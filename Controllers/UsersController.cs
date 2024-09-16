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
    public class UsersController : Controller
    {
        private dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(Users user)
        {
            if (user != null)
            {
                dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
                Users myuser = db.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
                if (myuser != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(user.password, myuser.password))
                    {
                        HttpCookie authCookie = new HttpCookie("auth", myuser.UserName);
                        HttpCookie roleCookie = new HttpCookie("role", myuser.role);

                        Response.Cookies.Add(authCookie);
                        Response.Cookies.Add(roleCookie);

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("Password", "Null");
            }
            return View();
        }
        public ActionResult Logout()
        {
            HttpCookie authCookie = new HttpCookie("auth");
            authCookie.Expires = DateTime.Now.AddDays(-1);

            HttpCookie roleCookie = new HttpCookie("role");
            roleCookie.Expires = DateTime.Now.AddDays(-1);

            Response.Cookies.Add(authCookie);
            Response.Cookies.Add(roleCookie);

            return RedirectToAction("Login");
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Users user, string retypePassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (user.password != retypePassword)
            {
                ModelState.AddModelError("retypePassword", "Passwords do not match.");
                return View();
            }

            dbQuanLiRuouEntities db = new dbQuanLiRuouEntities();
            Users myUser = db.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
            if (myUser != null)
            {
                ModelState.AddModelError("UserName", "UserName already exist.");
                return View();
            }

            myUser = db.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            if (myUser != null)
            {
                ModelState.AddModelError("EmailAddress", "Email already exist.");
                return View();
            }

            myUser = new Users();
            myUser.UserName = user.UserName;
            myUser.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            myUser.Email = user.Email;
            myUser.role = "user";
            db.Users.Add(myUser);
            db.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}
