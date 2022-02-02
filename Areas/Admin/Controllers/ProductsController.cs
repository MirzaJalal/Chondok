using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Chondok.Data;
using Chondok.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Chondok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;

        public ProductsController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }

        //GET Index
        public IActionResult Index()
        {
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            return View(product);
        }
        //POST Index
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                            .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            
            if(lowAmount==null || largeAmount == null)
            {
                product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            }
            return View(product);
        }

        //Counter
        public IActionResult Counter(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            
            return View(product);
        }

        //GET Create
        public IActionResult create()
        {
            ViewData["ProductTypeId"] = new SelectList(_db.productTypes.ToList(), "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(_db.specialTags.ToList(), "Id", "TagName");

            return View();
        }
        //POST Create
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.products.FirstOrDefault(c => c.Name == product.Name);
                if(searchProduct != null)
                {
                    ViewBag.message = "This product already exists!";
                    ViewData["ProductTypeId"] = new SelectList(_db.productTypes.ToList(), "Id", "ProductType");
                    ViewData["SpecialTagId"] = new SelectList(_db.specialTags.ToList(), "Id", "TagName");
                    return View(product);
                }

                if(image !=null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "images/noImage.PNG";
                }

                var prod = _db.products.Add(product);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product has been added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        //GET Edit
        public ActionResult Edit(int? id)
        {
            ViewData["ProductTypeId"] = new SelectList(_db.productTypes.ToList(), "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(_db.specialTags.ToList(), "Id", "TagName");

            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //POST Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile image)
        {
            //if (ModelState.IsValid)
            //{
            //    var searchProduct = _db.products.FirstOrDefault(c => c.Name == product.Name);
            //    if(searchProduct != null)
            //    {
            //        ViewBag.message = "This product already exists!";
            //        ViewData["ProductTypeId"] = new SelectList(_db.productTypes.ToList(), "Id", "ProductType");
            //        ViewData["SpecialTagId"] = new SelectList(_db.specialTags.ToList(), "Id", "TagName");
            //        return View(product);
            //    }

                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "images/noImage.png";
                }

                var prod = _db.products.Update(product);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product details updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //GET DETAILS
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //GET DELETE
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).SingleOrDefault(c => c.Id == id);
            if(product == null)
            {
                return BadRequest();
            }
            return View(product);
        }

        //POST DELETE
        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            _db.products.Remove(product);
            TempData["del"] = "Product has been removed!";
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
