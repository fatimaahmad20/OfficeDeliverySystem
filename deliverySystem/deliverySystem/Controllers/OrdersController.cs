using deliverySystem.Authentication;
using deliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace deliverySystem.Controllers
{
    [CustomAuthorize(Roles = "Admin, Client, DeliveryMan")]
    public class OrdersController : Controller
    {
        OfficeDeliveryContext myDB = new OfficeDeliveryContext();

        [CustomAuthorize(Roles = "Admin, Client")]
        public ActionResult MakeOrder(CategoryFilter category = CategoryFilter.All)
        {
            List<Item> items = new List<Item>();

            switch (category) {
                case CategoryFilter.All: items = myDB.Items.ToList(); break;
                case CategoryFilter.Cafeteria: items = myDB.Items.Where(i=>i.Category == Category.Cafeteria).ToList(); break;
                case CategoryFilter.Printer: items = myDB.Items.Where(i => i.Category == Category.Printer).ToList(); break;
            }
            
            return View(items);
        }

        public ActionResult Details(int id) { 
            Order order = myDB.Orders.Find(id);
            return View(order);
        }

        //list the orders 
        public ActionResult Orderlist()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var currentUser = myDB.Users.Find(user.UserId);

            List<Order> orders = new List<Order>();

            switch (currentUser.Role){
                case Role.Admin:
                    orders = myDB.Orders.Include(o=>o.User).Include(o => o.PrinterDeliveryMan).Include(o => o.CafeteriaDeliveryMan).ToList();
                    break;
                case Role.Client:
                    orders = myDB.Orders.Include(o => o.User).Include(o => o.PrinterDeliveryMan).Include(o => o.CafeteriaDeliveryMan).Where(order => order.User.Id == user.UserId).ToList();
                    break;
                case Role.DeliveryMan:
                    if (currentUser.Category == Category.Cafeteria) 
                        orders = myDB.Orders.Include(o => o.User).Where(order => order.CafeteriaDeliveryMan.Id == user.UserId && order.CafeteriaState == OrderState.Transit).ToList();
                    else
                        orders = myDB.Orders.Include(o => o.User).Where(order => order.PrinterDeliveryMan.Id == user.UserId && order.PrinterState == OrderState.Transit).ToList();
                    break;
            }

            return View(orders);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = myDB.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = myDB.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            myDB.Orders.Remove(order);
            myDB.SaveChanges();
            return RedirectToAction("orderList");
        }

        //list the items found
        public ActionResult viewitem(Category category)
        {
            var allitems = myDB.Items.Where(i => i.Category == category).ToList();
            return View(allitems);
        }

        //add to cart the items 
        [CustomAuthorize(Roles = "Admin, Client")]
        public ActionResult AddtoCart(int itemId)
        {
            if (Session["cart"] == null)
            {
                List<OrderItem> cart = new List<OrderItem>();
                Item item = myDB.Items.Find(itemId);

                cart.Add(new OrderItem()
                {
                    Item = item,
                    Quantity = 1
                });
                Session["cart"] = cart;
            }
            else
            {
                List<OrderItem> cart = (List<OrderItem>)Session["cart"];
                OrderItem orderItem = cart.Find(ci => ci.Item.Id == itemId);

                if (orderItem == null)
                {
                    Item item = myDB.Items.Find(itemId);
                    cart.Add(new OrderItem()
                    {
                        Item = item,
                        Quantity = 1
                    });
                }
                else {
                    orderItem.Quantity += 1;
                }

                Session["cart"] = cart;
            }

            return RedirectToAction("MakeOrder");
        }

        //remove the item from cart
        [CustomAuthorize(Roles = "Admin, Client")]
        public ActionResult Remove(int itemId)
        {
            List<OrderItem> cart = (List<OrderItem>)Session["cart"];
            OrderItem orderItem = cart.Find(ci => ci.Item.Id == itemId);
            if (orderItem != null) {
                orderItem.Quantity -= 1;

                if (orderItem.Quantity <= 0)
                    cart.Remove(orderItem);

                Session["cart"] = cart;
            }

            return RedirectToAction("MyOrder");
        }

        //list the items added to cart
        [CustomAuthorize(Roles = "Admin, Client")]
        public ActionResult MyOrder()
        {
            List<OrderItem> cart = (List<OrderItem>)Session["cart"];
            if (cart == null)
            {
                cart = new List<OrderItem>();
            }

            return View(cart);
        }

        [CustomAuthorize(Roles = "Admin, DeliveryMan")]
        public ActionResult CloseOrder(int id) {

            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var currentUser = myDB.Users.Find(user.UserId);

            Order order = myDB.Orders.Where(o => o.Id == id).
                Include(o => o.CafeteriaDeliveryMan).Include(o => o.PrinterDeliveryMan).Include(o => o.User).FirstOrDefault();
            if(order == null)
                return RedirectToAction("OrderList");

            if (currentUser.Role == Role.Admin)
            {
                if (order.CafeteriaDeliveryMan != null)
                    order.CafeteriaDeliveryMan.DeliveryFree = true;
                order.CafeteriaState = OrderState.Delivered;

                if (order.PrinterDeliveryMan != null)
                    order.PrinterDeliveryMan.DeliveryFree = true;
                order.PrinterState = OrderState.Delivered;
            }
            else
            {
                switch (currentUser.Category)
                {
                    case Category.Cafeteria:
                        if (order.CafeteriaDeliveryMan != null)
                            order.CafeteriaDeliveryMan.DeliveryFree = true;
                        order.CafeteriaState = OrderState.Delivered;
                        break;
                    case Category.Printer:
                        if (order.PrinterDeliveryMan != null)
                            order.PrinterDeliveryMan.DeliveryFree = true;
                        order.PrinterState = OrderState.Delivered;
                        break;
                }
            }

            myDB.SaveChanges();

            if (currentUser.Role == Role.Admin)
            {
                CheckPendingOrders(Category.Cafeteria);
                CheckPendingOrders(Category.Printer);
            }
            else
            {
                CheckPendingOrders((Category)currentUser.Category);
            }

            return RedirectToAction("OrderList");
        }

        private void CheckPendingOrders(Category category)
        {
            Order order = null; ;

            if (category == Category.Cafeteria)
            {
                order = (from o in myDB.Orders
                         where category == Category.Printer &&
                         o.CafeteriaState == OrderState.Waiting &&
                         o.Items.Where(i => i.Item.Category == Category.Cafeteria).Count() > 0
                         select o).FirstOrDefault();
            }
            else if (category == Category.Printer)
            {
                order = (from o in myDB.Orders
                         where category == Category.Printer &&
                         o.PrinterState == OrderState.Waiting &&
                         o.Items.Where(i => i.Item.Category == Category.Printer).Count() > 0
                         select o).FirstOrDefault();
            }

            if (order == null)
                return;

            var user = getFreeDeliveryMan(category);
            if (user != null)
            {
                user.DeliveryFree = false;
                if (category == Category.Cafeteria)
                {
                    order.CafeteriaDeliveryMan = user;
                    order.CafeteriaState = OrderState.Transit;
                }
                else if (category == Category.Printer)
                {
                    order.PrinterDeliveryMan = user;
                    order.PrinterState = OrderState.Transit;
                }
                myDB.SaveChanges();
            }
        }

        [CustomAuthorize(Roles = "Admin, Client")]
        public ActionResult Checkout()
        {
            List<OrderItem> cart = (List<OrderItem>)Session["cart"];

            if (cart == null)
                return RedirectToAction("OrderList");

            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var currentUser = myDB.Users.Find(user.UserId);

            Order order = new Order
            {
                DateTime = DateTime.Now,
                User = currentUser
            };

            foreach(OrderItem orderItem in cart) {
                orderItem.Order = order;
                orderItem.Item = myDB.Items.Find(orderItem.Item.Id);
                myDB.OrderItems.Add(orderItem);
            }
            myDB.SaveChanges();

            if (cart.Where(c => c.Item.Category == Category.Cafeteria).Count() > 0) {
                var cageteriaUser = getFreeDeliveryMan(Category.Cafeteria);
                if (cageteriaUser != null)
                {
                    order.CafeteriaDeliveryMan = cageteriaUser;
                    order.CafeteriaState = OrderState.Transit;
                    cageteriaUser.DeliveryFree = false;
                }
                else {
                    order.CafeteriaState = OrderState.Waiting;
                }
                myDB.SaveChanges();
            }

            if (cart.Where(c => c.Item.Category == Category.Printer).Count() > 0) {
                var printerUser = getFreeDeliveryMan(Category.Printer);
                if (printerUser != null)
                {
                    order.PrinterDeliveryMan = printerUser;
                    order.PrinterState = OrderState.Transit;
                    printerUser.DeliveryFree = false;
                }
                else {
                    order.PrinterState = OrderState.Waiting;
                }
                myDB.SaveChanges();
            }

            Session["cart"] = null;

            return RedirectToAction("OrderList");
        }

        private User getFreeDeliveryMan(Category category) { 
        
            User user = (from u in myDB.Users
                                   where u.Role == Role.DeliveryMan & u.Category == category & u.DeliveryFree
                                   select u).FirstOrDefault();
            
            return user;        
        }
    }
}