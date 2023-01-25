using deliverySystem.Authentication;
using deliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace deliverySystem.Controllers
{
    public class HomeController : Controller
    {
        OfficeDeliveryContext myDB = new OfficeDeliveryContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}