using deliverySystem.Authentication;
using deliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace deliverySystem.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        OfficeDeliveryContext myDB = new OfficeDeliveryContext();

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult usersList()
        {
            return View(myDB.Users.ToList());
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var user = myDB.Users.SingleOrDefault(m => m.Id == Id);

            if (user == null)
                return HttpNotFound();

            return View(user);


        }

        [HttpGet]
        public ActionResult Add()
        {
            Register register = new Register()
            {
                User = new User(),
                Departments = myDB.Departments.ToList()
            };
            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Register account)
        {
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
                string message = "User Created Successfully";
                ViewBag.Message = message;
            }
            return RedirectToAction("usersList");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = myDB.Users.Where(u => u.Id == id).SingleOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                myDB.Entry(user).State = EntityState.Modified;
                myDB.SaveChanges();
                return RedirectToAction("usersList");
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = myDB.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = myDB.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            List<Order> userOrders = new List<Order>();
            foreach(var order in myDB.Orders.Include(o=>o.User))
            {
                if (order.User.Id == user.Id) {
                    userOrders.Add(order);
                }
            }

            userOrders.ForEach(o=> myDB.Orders.Remove(o));
            myDB.Users.Remove(user);
            myDB.SaveChanges();
            return RedirectToAction("usersList");
        }

        public ActionResult viewDeliveryOrder()
        {
            var user = myDB.Users.FirstOrDefault(u => u.Email == Request.Cookies["email"].Value);
            var allorders = myDB.Items.Where(i => i.Category == user.Category).ToList();

            return View(allorders);
        }
    }
}