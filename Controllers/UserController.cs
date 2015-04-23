using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApplication10.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult logIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult logIn(Models.UserModel user)
        {
            if (Isvalid(user.Email, user.Password))
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return RedirectToAction("List", "User");
            }
            else
            {
                ModelState.AddModelError("", "Login data in incorrect");
            }
            return View(user);
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Models.UserModel user)
        {
            if (ModelState.IsValid )
            {
                using (var db = new TestingEntities())
                {
                    var sysUser = db.USERS.Create();
                    sysUser.NAME = user.Name;
                    sysUser.EMAIL = user.Email;
                    sysUser.PASSWORD = user.Password;
                    DateTime date = Convert.ToDateTime(user.BirthDay);
                    sysUser.BIRTHDAY = date;
                    sysUser.COUNTRY = user.Country;
                    sysUser.AVATAR = null;

                    db.USERS.Add(sysUser);
                    db.SaveChanges();
                    int lastId = db.USERS.Max(item => item.ID);
                    InsertLogin(lastId);
                    FormsAuthentication.SetAuthCookie(user.Email, false);

                    return RedirectToAction("List", "User");                    
                }
            }
            else
            {
                ModelState.AddModelError("", "Login data is incorrect.");
            }
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private bool Isvalid(string Email, string password)
        {
            bool Isvalid = false;
            using (var db = new TestingEntities())
            {
                var user = db.USERS.FirstOrDefault(u => u.EMAIL == Email  && u.PASSWORD == password);
                if (user !=null)
                {                    
                    Isvalid = true;
                    InsertLogin(user.ID);                   
                }
            }
            return Isvalid;
        }

        private void InsertLogin(int idUser)
        {
            using (var db = new TestingEntities())
            {
                var register = db.REGISTER.FirstOrDefault(u => u.IDUSER == idUser);
                if (register != null)
                {
                    register.DLOGIN = DateTime.Now;
                    register.CONNECT = true;
                    register.COUNT = (register.COUNT + 1);
                } 
                else 
                {
                    var registerNew = db.REGISTER.Create();
                    registerNew.CONNECT = true;
                    registerNew.DLOGIN = DateTime.Now;
                    registerNew.IDUSER = idUser;
                    db.REGISTER.Add(registerNew);                    
                }
                db.SaveChanges();               
            }
        }

        public ActionResult List()
        {
            var users = new List<USERS>();
            using (TestingEntities dc = new TestingEntities())
            {
                users = dc.USERS.ToList();
            }
            return View(users);
            /*
            using (TestingEntities dc = new TestingEntities())
            {
                var users = dc.USERS.Join(dc.REGISTER, u => u.ID, r => r.IDUSER, (u, r) => new { USERS = u, REGISTER = r }).ToList();
                return View(users);
            }
            */
        }
    }
}