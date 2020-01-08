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
using Microsoft.AspNetCore.Mvc;

namespace JanaFood.Controllers
{
    
    public class HomeController : Controller
    {
        private IFoodData _foodData;
        private IWebHostEnvironment _hostingEnvironment;

        public HomeController(IFoodData foodData, IWebHostEnvironment hostingEnvironment)
        {
            _foodData = foodData;
            _hostingEnvironment = hostingEnvironment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
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

                return RedirectToAction(nameof(Details), new {id = newFood.Id });
            }
            else
            {
                return View();
            }
            
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var model = _foodData.GetFood(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
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