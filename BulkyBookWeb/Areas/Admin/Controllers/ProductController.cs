using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {

        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitofWork unitofWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitofWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get
        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                Product = new(),

                CategoryList = _unitofWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                CoverTypeList = _unitofWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                //Create Product
                return View(productVM);
            }
            else
            {
                //Edit Product
                productVM.Product = _unitofWork.Product.GetFirstorDefault(u => u.Id == id);
                return View(productVM);
            }
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {
           
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                //If file already exists
                if(file != null) 
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if(obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                           
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if(obj.Product.Id == 0)
                {
                    _unitofWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitofWork.Product.Update(obj.Product);
                }
                _unitofWork.Save();
                TempData["success"] = "Product Created Successully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //API CALLS REGION 

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitofWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });

        }

        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {
            var obj = _unitofWork.Product.GetFirstorDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitofWork.Product.Remove(obj);
            _unitofWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
