using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CoverTypeController : Controller
    {

        private readonly IUnitofWork _unitofWork;
        public CoverTypeController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;

        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverType = _unitofWork.CoverType.GetAll();
            return View(objCoverType);
        }

        //Get
        public IActionResult Create()
        {

            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            
            if (ModelState.IsValid)
            {
                _unitofWork.CoverType.Add(obj);
                _unitofWork.Save();
                TempData["success"] = "CoverType Created Successully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var CoverTypeForDb = _unitofWork.CoverType.GetFirstorDefault(u => u.Id == id);
            if (CoverTypeForDb == null)
            {
                return NotFound();
            }
            return View(CoverTypeForDb);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
           
            if (ModelState.IsValid)
            {
                _unitofWork.CoverType.Update(obj);
                _unitofWork.Save();
                TempData["success"] = "CoverType Updated Successully";
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
            var CoverTypeForDb = _unitofWork.CoverType.GetFirstorDefault(u => u.Id == id);
            if (CoverTypeForDb == null)
            {
                return NotFound();
            }

            return View(CoverTypeForDb);
        }

        //post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitofWork.CoverType.GetFirstorDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitofWork.CoverType.Remove(obj);
            _unitofWork.Save();
            TempData["success"] = "CoverType Deleted Successully";
            return RedirectToAction("Index");
        }
    }
}
