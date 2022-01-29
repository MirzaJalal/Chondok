using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chondok.Areas.Admin.ViewModel;
using Chondok.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Chondok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _db;
        public RolesController(RoleManager<IdentityRole> roleManager, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Index()
        {
            var role = _roleManager.Roles.ToList();
            ViewBag.Roles = role;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name) //this name variable must be same as "name=name" in the create view
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(role.Name);

            if(isExist == true)
            {
                ViewBag.message = "This role is already exist";
                ViewBag.name = name;
                return View();
            }

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                TempData["save"] = "New Role created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if(role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name) //this name variable must be same as "name=name" in the create view
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;

            var isExist = await _roleManager.RoleExistsAsync(role.Name);

            if (isExist == true)
            {
                ViewBag.message = "This role is already exist";
                ViewBag.name = name;
                return View();
            }

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                TempData["save"] = "Role has been Updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View(role);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }
            
            var result = _roleManager.DeleteAsync(role);


            if (result != null)
            {
                TempData["del"] = "Role has been Deleted!";

                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        public async Task<IActionResult> Assign()
        {
            ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(c => c.LockoutEnd < DateTime.Now || c.LockoutEnd ==null).ToList(), "Id", "UserName"); //users who are not in lockout state
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(RoleUser roleUser)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == roleUser.UserId);

            var role = await _userManager.AddToRoleAsync(user, roleUser.RoleId);

            var count = await _userManager.IsInRoleAsync(user, roleUser.RoleId);
            if (count != null)
            {
                ViewBag.message = "This user already is in role";
                ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(c => c.LockoutEnd < DateTime.Now || c.LockoutEnd == null).ToList(), "Id", "UserName"); //users who are not in lockout state
                ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");

                return View();
            }
            if (role.Succeeded)
            {  
                TempData["save"] = "Role has been assigned successfully!";
                return RedirectToAction(nameof(Index));
            }
            
            return View();
        }
        public ActionResult AssignUserRole()
        {
            var result = from usr in _db.UserRoles
                         join r in _db.Roles on usr.RoleId equals r.Id
                         join a in _db.ApplicationUsers on usr.UserId equals a.Id
                         select new UserRoleMapping()
                         {
                             UserId = usr.UserId,
                             RoleId = usr.RoleId,
                             UserName = a.UserName,
                             RoleName = r.Name
                         };
            ViewBag.UserRoles = result;
            return View();
        }
    }
}
