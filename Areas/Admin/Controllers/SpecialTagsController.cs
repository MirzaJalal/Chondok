using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chondok.Data;
using Chondok.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chondok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagsController : Controller
    {
        private ApplicationDbContext _db;

        public SpecialTagsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var list = _db.specialTags.ToList();
            return View(list);
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTag specialTag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _db.Add(specialTag);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Edit GET action Method
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.specialTags.Find(id);
            if (specialTag == null)
            {
                return BadRequest();
            }
            return View(specialTag);
        }
        //POST Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _db.Update(specialTag);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(specialTag);
        }
        //GET:: DETAILS
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.specialTags.Find(id);
            if (specialTag == null)
            {
                return BadRequest();
            }
            return View(specialTag);
        }
        //POST:: DETAILS Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(SpecialTag specialTag)
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
            var specialTag = _db.specialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        //POST:: DETAILS Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, SpecialTag specialTag)
        {
            var data = _db.specialTags.Find(id);
            _db.Remove(data);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
