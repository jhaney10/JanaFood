using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JanaFood.Models;
using JanaFood.Services;
using JanaFood.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JanaFood.Controllers
{
    
    public class HomeController : Controller
    {
        private IFoodData _foodData;
        private IWebHostEnvironment _hostingEnvironment;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public HomeController(IFoodData foodData, IWebHostEnvironment hostingEnvironment, 
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _foodData = foodData;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                ViewBag.UserId = _userManager.GetUserId(User);
            }

            var viewModel = new HomeViewModel();
            viewModel.Foods = _foodData.GetAll();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateFoodViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {


                    string[] permittedExtensions = { ".jpg", ".png", ",jpeg" };
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    var ext = Path.GetExtension(uniqueFileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                    {
                        return View();
                    }
                    else
                    {
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    
                }
                var newFood = new Food();
                newFood.Name = model.Name;
                newFood.Description = model.Description;
                newFood.Price = model.Price;
                newFood.Picture = uniqueFileName;

                var result = _foodData.SaveFood(newFood);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                //ViewBag.UserId = _userManager.GetUserId(User);
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);
                var food = _foodData.GetFood(id);
                if (food == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var orderFoodModel = new OrderFoodViewModel
                {
                    Customer = user,
                    CustomerOrder = food,
                };
                return View(orderFoodModel);
            }
            else
            {
                return RedirectToAction("Account", "Login");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Order(OrderFoodViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Customer.Id);
            var food = _foodData.GetFood(model.CustomerOrder.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User Id {user} was not found";
                return View("Not Found");
            }
            else if(food == null)
            {
                ViewBag.ErrorMessage = $"The Food Id {food} was not found";
                return View("Not Found");
            }
            else
            {
                var newOrder = new Order
                {
                    Customer = user,
                    CustomerOrder = food,
                    OrderDate = DateTime.Now,
                    DeliveryAddress = model.DeliveryAddress,
                    OrderStatus = "Pending Delivery",
                };
                var orderFood = _foodData.OrderFood(newOrder);
                TempData["Message"] = "Order Successful";
                return RedirectToAction("Index","Home");
            }

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var foodDetails = _foodData.GetFood(id);

            if(foodDetails == null)
            {
                return View(nameof(Index));
            }
            else
            {
                return View(foodDetails);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDetails(CreateFoodViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newFoodDetails = new Food();
                newFoodDetails.Name = model.Name;
                newFoodDetails.Description = model.Description;
                newFoodDetails.Price = model.Price;
                newFoodDetails.Id = model.Id;

                var updateDetails = _foodData.UpdateFood(newFoodDetails);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFood(int id)
        {
            if (ModelState.IsValid)
            {
                var response = _foodData.DeleteFood(id);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }
    }
}