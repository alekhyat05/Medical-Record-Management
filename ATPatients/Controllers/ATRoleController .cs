using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATPatients.Data;
using ATPatients.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATPatients.Controllers
{
    //  [Authorize(Roles = "Administrators,Administrator")]
    public class ATRoleController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly PatientsContext _context;
        private readonly ApplicationDbContext _db;

        public ATRoleController(
              UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, PatientsContext context, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _context = context;
            _db = db;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var roles = roleManager.Roles.OrderBy(m => m.Name);
            //var roles = _db.Roles;
            return View(roles);
        }

        //public IActionResult Index()
        //{
        //    var roles = roleManager.Roles.OrderBy(m => m.Name);
        //    return View(roles);

        //}


        [HttpPost]
        public async Task<IActionResult> CreateRole(String roleNameInput)
        {
            try
            {
                roleNameInput = roleNameInput.Trim();
                if (roleNameInput == "" || roleNameInput == null)
                {
                    TempData["medicationData"] = "Name cannot be blank";
                    return RedirectToAction("Index");
                }
                if (ModelState.IsValid)
                {
                    IdentityRole role = new IdentityRole { Name = roleNameInput };


                    var result = await roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        //return RedirectToAction("ListRole");
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateRoleName")
                        {
                            TempData["medicationData"] = "Name is Already on File";
                            return RedirectToAction("Index");
                        }
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["medicationData"] = ex.InnerException.ToString();
                return RedirectToAction("Index");
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListRole()
        {
            var roles = roleManager.Roles.OrderBy(m => m.Name);

            //   var roles = roleManager.Roles.OrderBy<roleManager.Roles>;
            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                // Error Mesage
            }
            var modelUsers = new List<IdentityUser>();

            if (role.Name == "Administrator")
            {
                TempData["medicationData"] = "Admin role cannot be removed";
                return RedirectToAction(nameof(Index));
            }

            foreach (var user in userManager.Users)
            {
                var userrole = new UserRole
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {

                    userrole.IsSelected = true;
                    modelUsers.Add(user);
                }
                // ViewBag.ListofUser = userrole.UserName;
            }

            if (modelUsers.Count == 0)
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    TempData["medicationData"] = "Role has been deleted";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    TempData["medicationData"] = "Error in deleting the role";
                    ModelState.AddModelError("", error.Description);
                }

            }

            ViewBag.RoleId = id;
            return View(modelUsers);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteRole(DeleteRole model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                // Error Mesage
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    TempData["mesasge"] = "Role has been deleted";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    TempData["message"] = "Failed deleting the role";
                    ModelState.AddModelError("", error.Description);
                }
            }


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId, string roleName)
        {
            ViewData["roleName"] = roleName;
            if (roleId != null)
            {
                Response.Cookies.Append("roleId", roleId, new CookieOptions { Expires = DateTime.Today.AddDays(2) });
            }

            ViewBag.roleId = roleId;
            // IdentityUser role = new IdentityUser { Name = roleNameInput };
            var role = await roleManager.FindByIdAsync(roleId);
          

            var model = new List<UserRole>();

            foreach (var user in userManager.Users)
            {
                var userrole = new UserRole
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userrole.IsSelected = true;
                }
                else
                {
                    userrole.IsSelected = false;
                }
                    
                model.Add(userrole);
                // ViewBag.ListofUser = userrole.UserName;
            }
            ViewBag.ListofUser = new SelectList(model.Where(m => m.IsSelected == false), "UserId", "UserName");
            return View(model.Where(m => m.IsSelected == true));
            //return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(string userSelected)
        {
            string roleId = Request.Cookies["roleId"];



            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                // Error meesage
                TempData["Message"] = $"Role with id = {roleId} cannot be found";
            }


            var user = await userManager.FindByIdAsync(userSelected);
            IdentityResult result = null;


            result = await userManager.AddToRoleAsync(user, role.Name);


            if (result.Succeeded)
            {
                TempData["Message"] = "User Added Successfully";
                return RedirectToAction("EditUsersInRole", new { roleId = roleId, roleName = role.Name });

            }
            TempData["Message"] = "Failed Adding User";

            return RedirectToAction("EditUsersInRole", new { roleId = roleId, roleName = role.Name });

        }

        //[HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            string roleId = Request.Cookies["roleId"];



            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                // Error meesage
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
            }


            var user = await userManager.FindByIdAsync(id);
            IdentityResult result = null;


            result = await userManager.RemoveFromRoleAsync(user, role.Name);//user record entire, from which role


            if (result.Succeeded)
            {
                TempData["Message"] = "User Removed Successfully";
                return RedirectToAction("EditUsersInRole", new { roleId = roleId, roleName = role.Name });

            }
            TempData["Message"] = "Failed Removing User";

            return RedirectToAction("EditUsersInRole", new { roleId = roleId, roleName = role.Name });
        }
    }
}
