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
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public ManagerController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"This User Id {id} does not exist";
                return View("Not Found");
            }
            else
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var userClaims = await _userManager.GetClaimsAsync(user);
                var editUserModel = new EditUserViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    City = user.City,
                    Roles = userRoles,
                    Claims = userClaims.Select(x => x.Value).ToList()

                };
                return View(editUserModel);
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"This User Id {model.UserId} does not exist";
                return View("Not Found");
            }
            else
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.City = model.City;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers", "Manager");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);

                }
            }


        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"This User Id {id} does not exist";
                return View("Not Found");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers", "Manager");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();

                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> ManageRolesInUser(string userId)
        {
            ViewBag.UserId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User Id {userId} was not found";
                return View("Not Found");
            }
            else
            {
                var userRoles = new List<ManageRolesViewModel>();
                foreach (var role in _roleManager.Roles)
                {
                    var roleView = new ManageRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                    };

                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        roleView.IsSelected = true;
                    }
                    userRoles.Add(roleView);
                }

                return View(userRoles);
            }

        }

        [HttpPost]
        public async Task<IActionResult> ManageRolesInUser(List<ManageRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User Id {userId} was not found";
                return View("Not Found");
            }
            else
            {
                for (int i = 0; i < model.Count; i++)
                {
                    IdentityResult result = null;
                    var checkRole = await _userManager.IsInRoleAsync(user, model[i].RoleName);
                    if (checkRole && !model[i].IsSelected)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model[i].RoleName);
                    }
                    else if(!checkRole && model[i].IsSelected)
                    {
                        await _userManager.AddToRoleAsync(user, model[i].RoleName);
                    }
                    else
                    {
                        continue;
                    }
                }
                return View(model);
            }

        }

    }
}
