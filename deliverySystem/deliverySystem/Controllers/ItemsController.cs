using deliverySystem.Authentication;
using deliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace deliverySystem.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ItemsController : Controller
    {
        OfficeDeliveryContext myDB = new OfficeDeliveryContext();

        [HttpGet]
        public ActionResult ItemsList()
        {
            var allItems = myDB.Items.ToList();
            return View(allItems);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Item item)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["File"];
                string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(file.FileName));
                item.File = file.FileName;
                file.SaveAs(path);

                myDB.Items.Add(item);
                myDB.SaveChanges();
            }
            return RedirectToAction("itemsList");
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var item = myDB.Items.Find(Id);

            if (item == null)
                return HttpNotFound();

            return View(item);


        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = myDB.Items.Where(u => u.Id == id).SingleOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                myDB.Entry(item).State = EntityState.Modified;
                myDB.SaveChanges();
                return RedirectToAction("itemsList");
            }
            return View(item);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = myDB.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = myDB.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            myDB.Items.Remove(item);
            myDB.SaveChanges();
            return RedirectToAction("itemsList");
        }

        //list the items found
        public ActionResult ViewCategoryItem(Category category)
        {
            var allitems = myDB.Items.Where(i => i.Category == category).ToList();
            return View(allitems);
        }
       
    }
}