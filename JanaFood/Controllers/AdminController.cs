using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JanaFood.Models;
using JanaFood.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JanaFood.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                }
                else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return View("Not Found");
            }
            else
            {
                var model = new EditRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                foreach (var user in _userManager.Users)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        model.Users.Add(user.UserName);
                    }
                }
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = "Role cannot be found";
                return View("Not Found");
            }
            else
            {
                role.Name = model.RoleName;
                var update = await _roleManager.UpdateAsync(role);
                if (update.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in update.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.RoleId = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role == null)
            {
                ViewBag.ErrorMessage = $"The Role Id {roleId} was not found";
                return View("Not Found");
            }
            else
            {
                var userRole = new List<UserViewModel>();
                foreach (var user in _userManager.Users)
                {
                    var userRoleView = new UserViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                    }; 

                    if(await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRoleView.IsSelected = true;
                    }
                    userRole.Add(userRoleView);
                }

                return View(userRole);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The Role Id {roleId} was not found";
                return View("Not Found");
            }
            else
            {
                for (int i=0; i < model.Count; i++)
                {
                    IdentityResult result = null;
                    var user = await _userManager.FindByNameAsync(model[i].UserName);
                    var checkRole = await _userManager.IsInRoleAsync(user, role.Name);

                    if (model[i].IsSelected && !checkRole)
                    {
                        result  = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if(!model[i].IsSelected && checkRole)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }

                    
                }
                return RedirectToAction("EditRole", new { id = roleId});
            }
                
        }
    }
}