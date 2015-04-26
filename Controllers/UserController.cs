using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;

namespace MvcApplication10.Controllers
{    

    public class UserController : Controller
    {
        private static Helpers.SessionManager _session;
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["TestingDB"].ConnectionString;

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
        public ActionResult logIn(SharedModels.Models.UserModel user)
        {
            Helpers.Utils.validateUser(user);
            ConnectionsManager.Users conn = new ConnectionsManager.Users(CONNECTIONSTRING);
            int idUser = conn.existUser(user.Email, user.Password);
            if ( idUser > 0)
            {
                if (_session == null)
                {
                    _session = new Helpers.SessionManager();
                    _session.setIsLoggedIn(user.Email);
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                }
                else
                {
                    _session.setIsLoggedIn(user.Email);
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                }
                conn.AddRegister(idUser, _session.GetSessionId(), Request.UserHostAddress);
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
        public ActionResult Registration(SharedModels.Models.UserModel user)
        {
            if (ModelState.IsValid )
            {
                Helpers.Utils.validateUser(user);
                user.IP = Request.UserHostAddress;
                ConnectionsManager.Users conn = new ConnectionsManager.Users(CONNECTIONSTRING);
                if (conn.existUser(user.Email, user.Password) > 0)
                {
                    ModelState.AddModelError("", "User exist with the same email and password");
                    return View();
                }
                else
                {
                    conn.AddUser(user);                    
                }
            }
            else
            {
                ModelState.AddModelError("", "Login data is incorrect.");
                return View();
            }
            return RedirectToAction("logIn", "User");
        }

        public ActionResult LogOut()
        {
            ConnectionsManager.Users conn = new ConnectionsManager.Users(CONNECTIONSTRING);
            if (_session != null)
            {
                conn.updateRegister(_session.GetSessionId());
            }
            else
            {
                _session = new Helpers.SessionManager();
                conn.updateRegister(_session.GetSessionId());
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult List()
        {
            
            ConnectionsManager.Users conn = new ConnectionsManager.Users(CONNECTIONSTRING);
            List<SharedModels.Models.ListUsers> listUsers = conn.getListUsers();
            return View(listUsers);
            /*
             * var users = new List<USERS>();
            using (TestingEntities dc = new TestingEntities())
            {
                users = dc.USERS.ToList();
            }
             * 
            using (TestingEntities dc = new TestingEntities())
            {
                var users = dc.USERS.Join(dc.REGISTER, u => u.ID, r => r.IDUSER, (u, r) => new { USERS = u, REGISTER = r }).ToList();
                return View(users);
            }
            */
        }

        public ActionResult Autocomplete(string term)
        {
            var items = new[] { "Apple", "Pear", "Banana", "Pineapple", "Peach" };

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

    }
}