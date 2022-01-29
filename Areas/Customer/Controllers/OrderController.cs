using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chondok.Data;
using Chondok.Models;
using Chondok.Utility;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Checkout(Order order)
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    order.OrderDetails.Add(orderDetails);
                }
            }
            order.OrderNo = GetOrderNo();
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("products", null);
            return View();
        }
        
        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count()+1;
            return rowCount.ToString("000");
        }
    }
}
