using deliverySystem.Authentication;
using deliverySystem.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace deliverySystem.Controllers
{
    [CustomAuthorize(Roles = "Admin, Client")]
    public class ReviewMessageController : Controller
    {
        OfficeDeliveryContext myDB = new OfficeDeliveryContext();

        public ActionResult MessageList()
        {   var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var currentUser = myDB.Users.Find(user.UserId);

            if(currentUser.Role == Role.Admin)
                return View(myDB.Messages.Include(m=>m.Sender).ToList());

            var messages = myDB.Messages.Where(m=> m.Sender.Id == currentUser.Id).ToList();
            return View(messages);
        }

        [CustomAuthorize(Roles = "Client")]
        public ViewResult SendMessage()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(Message message)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var currentUser = myDB.Users.Find(user.UserId);

            message.Time = DateTime.Now;
            message.Sender = currentUser;
            myDB.Messages.Add(message);
            myDB.SaveChanges();
            return RedirectToAction("messagelist");
        }
    }
}