using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Chondok.Data;
using Chondok.Models;
using Chondok.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Chondok.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET Checkout
        public IActionResult Checkout()
        {
            return View();
        }

        //POST Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order, Product pr)
        {
                List<Product> products = HttpContext.Session.Get<List<Product>>("products");
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        OrderDetails orderDetails = new OrderDetails();
                        var x = orderDetails.ProductId = product.Id;

                    order.OrderDetails.Add(orderDetails);
                    }
                }
                order.OrderNo = GetOrderNo();
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
            pr.StockInNo--;
            _db.SaveChanges();
            HttpContext.Session.Set("products", new List<Product>());


            return View();
            //return RedirectToAction("Index", "HomeController");
        }

        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count()+1;
            return rowCount.ToString("000");
        }
    }
}
