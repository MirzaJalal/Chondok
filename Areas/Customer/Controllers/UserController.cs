using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chondok.Data;
using Chondok.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chondok.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;

        private ApplicationDbContext _db;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Index()
        {
            var user = _db.ApplicationUsers.ToList();
            return View(user);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    var isSaveRole = await _userManager.AddToRoleAsync(user,"user"); //parameter(user,"user_defined role")
                    TempData["save"] = "User has been created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user== null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ApplicationUser applicationUser)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);

            if(user == null)
            {
                return NotFound();
            }
            user.FirstName = applicationUser.FirstName;
            user.LastName = applicationUser.LastName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["save"] = "The information Updated Successfully";
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        public IActionResult Details(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        public async Task<IActionResult> Lockout(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUsers.SingleOrDefault(c => c.Id == id);
            if(user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Lockout(ApplicationUser applicationUser)
        {
            var user = _db.ApplicationUsers.SingleOrDefault(c => c.Id == applicationUser.Id);
            if(user == null)
            {
                return NotFound();
            }

            user.LockoutEnd = DateTime.Now.AddDays(10);
            var rowAffected = await _db.SaveChangesAsync();
            if (rowAffected > 0)
            {
                TempData["del"] = "User has been lockout successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Active(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUser applicationUser)
        {
            var user = _db.ApplicationUsers.SingleOrDefault(c => c.Id == applicationUser.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.LockoutEnd = DateTime.Now.AddDays(-1);
            var rowAffected = await _db.SaveChangesAsync();
            if (rowAffected > 0)
            {
                TempData["save"] = "User has removed from lockout state";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        public async Task<IActionResult> Delete(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser applicationUser)
        {
            var user = _db.ApplicationUsers.SingleOrDefault(c => c.Id == applicationUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            _db.ApplicationUsers.Remove(user);
            var rowAffected = await _db.SaveChangesAsync();
            if (rowAffected > 0)
            {
                TempData["del"] = "User has been removed";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
