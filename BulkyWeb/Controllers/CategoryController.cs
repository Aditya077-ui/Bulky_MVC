using Bulky.DataAccess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _db;
        public CategoryController(ApplicationDBContext db)
        {
            _db = db;
        }
        //CREATE SECTION
        public IActionResult Index()
        {                                                              //get all categories
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()     //by default it is the get method
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)  // create category items
        {
            if (obj.Name == obj.DisplayOrder.ToString()) 
           {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }
         // EDIT SECTION
        public IActionResult Edit(int? id)     //by default it is the get method
        {
            if (id == null || id == 0) 
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)  // edit category items
        {
          
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }


        //delete section
        public IActionResult Delete(int? id)     //by default it is the get method
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id)  // edit category items
        {
            {
                Category? obj = _db.Categories.Find(id);
                if (obj == null)
                {
                    return NotFound();
                }
                _db.Categories.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction("Index");
            }
            
            

        }
    }
}
