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
    [Area("Admin")]
    public class CompanyController : Controller
    {

        private readonly IUnitofWork _unitofWork;
        public CompanyController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            
            if (id == null || id == 0)
            {
                return View(company);
            }

            else
            {
                //Edit Product
                company = _unitofWork.Company.GetFirstorDefault(u => u.Id == id);
                return View(company);
            }
        }

            
        
    
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
           
            if (ModelState.IsValid)
            {
                if(obj.Id == 0)
                {
                    _unitofWork.Company.Add(obj);
                    TempData["success"] = "Product Created Successully";
                }
                else
                {
                    _unitofWork.Company.Update(obj);
                    TempData["success"] = "Product Updated Successully";
                }
                _unitofWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //API CALLS REGION 

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitofWork.Company.GetAll();
            return Json(new { data = companyList });

        }

        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {
            var obj = _unitofWork.Company.GetFirstorDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitofWork.Company.Remove(obj);
            _unitofWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
