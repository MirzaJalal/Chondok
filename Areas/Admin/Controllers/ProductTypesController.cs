using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chondok.Data;
using Chondok.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chondok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductTypesController : Controller
    {
        private ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var data = _db.productTypes.ToList();

            return View(data);
        }

        //Create GET action Method 
        public ActionResult Create()
        {

            return View();
        }
        //POST Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.productTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product type saved successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }


        //Edit GET action Method
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.productTypes.Find(id);
            if (productType == null)
            {
                return BadRequest();
            }
            return View(productType);
        }
        //POST Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product type updated!";
                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }
        //GET:: DETAILS
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.productTypes.Find(id);
            if (productType == null)
            {
                return BadRequest();
            }
            return View(productType);
        }
        //POST:: DETAILS Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes productTypes)
        {

            return RedirectToAction(nameof(Index));
        }

        //GET:: DELETE
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var productType = _db.productTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }
        //POST:: DETAILS Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, ProductTypes productTypes)
        {
            var productType = _db.productTypes.Find(id);
            _db.Remove(productType);
            _db.SaveChanges();
            TempData["del"] = "Product type deleted!";
            return RedirectToAction(nameof(Index));
        }
    }
}
