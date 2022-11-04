using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Dataccess;
using BulkyBook.Models;

using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public CategoryController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
                          
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategory = _unitofWork.Category.GetAll();
            return View(objCategory);
        }

        //Get
        public IActionResult Create()
        {
            
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOder cannot match exactly the Name");
            }
            if (ModelState.IsValid)
            {
                _unitofWork.Category.Add(obj);
                _unitofWork.Save();
                TempData["success"] = "Category Created Successully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //Get
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var categoryForDb = _unitofWork.Category.GetFirstorDefault(u => u.Id == id);
            if(categoryForDb == null)
            {
                return NotFound();
            }
            return View(categoryForDb);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOder cannot match exactly the Name");
            }
            if (ModelState.IsValid)
            {
                _unitofWork.Category.Update(obj);
                _unitofWork.Save();
                TempData["success"] = "Category Updated Successully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // var categoryForDb = _Db.Categories.Find(id);
            var categoryForDb = _unitofWork.Category.GetFirstorDefault(u => u.Id == id);
            if (categoryForDb == null)
            {
                return NotFound();
            }

            return View(categoryForDb);
        }

        //post
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitofWork.Category.GetFirstorDefault(u => u.Id == id);
            if ( obj == null)
            {
                return NotFound();
            }
            _unitofWork.Category.Remove(obj);
            _unitofWork.Save();
            TempData["success"] = "Category Deleted Successully";
            return RedirectToAction("Index");
        }
    }
}
