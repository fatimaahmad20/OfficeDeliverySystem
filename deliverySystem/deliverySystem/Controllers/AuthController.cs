using deliverySystem.Authentication;
using deliverySystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace deliverySystem.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        OfficeDeliveryContext myDB = new OfficeDeliveryContext();
       
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account, string ReturnUrl = "")
        {

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(account.Email, account.Password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(account.Email, false);
                    var currentUser = myDB.Users.Where(u => u.Email == account.Email).FirstOrDefault();
                    if (user != null && currentUser != null)
                    {
                        CustomSerializeModel userModel = new CustomSerializeModel()
                        {
                            UserId = currentUser.Id,
                            Role = Enum.GetName(typeof(Role), user.Roles),
                            Email = user.Email
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                        //    (
                        //    1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                        //    );

                        //string enTicket = FormsAuthentication.Encrypt(authTicket);
                        //HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                        HttpCookie faCookie = new HttpCookie("Cookie1", userData);
                        Response.Cookies.Add(faCookie);
                    }

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Something Wrong : Username or Password invalid ^_^ ");
            return View(account);
        }

        [HttpGet]
        public ActionResult Register()
        {
            Register register = new Register()
            {
                User = new User(),
                Departments = myDB.Departments.ToList()
            };
            return View(register);
        }

        [HttpPost]
        public ActionResult Register(Register account)
        {
            bool statusRegistration = false;
            string messageRegistration;

            if (ModelState.IsValid)
            {
                account.User.Department = myDB.Departments.Find(account.SelectedDepartmentId);
                // Email Verification
                string email = Membership.GetUserNameByEmail(account.User.Email);
                if (!string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError("Warning Email", "Sorry: Email already Exists");
                    ViewBag.Message = "Sorry: Email already Exists";
                    account.Departments = myDB.Departments.ToList();
                    return View(account);
                }

                myDB.Users.Add(account.User);
                myDB.SaveChanges();

                messageRegistration = "Your account has been created successfully. ^_^";
                statusRegistration = true;
            }
            else
            {
                account.Departments = myDB.Departments.ToList();
                messageRegistration = "Something Wrong!";
            }

            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;

            return View(account);
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth", null);
        }

    }
}