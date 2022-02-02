using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chondok.Models;
using Chondok.Data;
using Microsoft.EntityFrameworkCore;
using Chondok.Utility;
using Microsoft.AspNetCore.Http;
using X.PagedList;

namespace Chondok.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int? page)
        {
            return View(_db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList().ToPagedList(page ?? 1, 8));
            //return View(_db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList().ToPagedList(page??1,12));
        }
        //GET Product DETAILS
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            
            if(product == null)
            {
                return BadRequest();
            }
            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        public IActionResult ProductDetails(int? id)
        {
            List<Product> products = new List<Product>();

            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return BadRequest();
            }
            products = HttpContext.Session.Get<List<Product>>("products");
            if(products == null)
            {
                products = new List<Product>();
            }
            products.Add(product);
            //var x = products.Count();
            if(product.StockInNo == 0)
            {
                ViewBag.message = "This product is out of Stock!";
                TempData["save"] = "This product is out of Stock!";
            }
            else
            {
                product.StockInNo--;
                _db.SaveChanges();
                HttpContext.Session.Set("products", products);
            }
            return View(product);

        }

        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", new List<Product>());
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", new List<Product>());
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //GET Product Cart
        public IActionResult Cart(Product product)
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");

            if(products == null)
            {
                products = new List<Product>();
            }

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
