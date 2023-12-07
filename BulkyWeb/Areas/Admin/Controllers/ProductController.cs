using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;       // to access wwwroot folder
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()              //get all products
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)  // get method         //combining update and insert 
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category   // for category list to display while creating product
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),

                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create 
                return View(productVM);
            }
            else
            {            //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }


        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            try

            {
               if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;  //gives the path to wwwroot folder
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);  //random name/guid  
                        string ProductPath = Path.Combine(wwwRootPath, @"images\product");    //location for images
                        if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                        {
                            //delete the old image 
                            var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        //upload new image
                        using (var filestream = new FileStream(Path.Combine(ProductPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }
                        productVM.Product.ImageUrl = @"\images\product\" + fileName;   //saving it into imageurl
                    }

                    if (productVM.Product.Id == 0)
                    {
                        _unitOfWork.Product.Add(productVM.Product);
                    }
                    else
                    {
                        _unitOfWork.Product.Update(productVM.Product);
                    }

                    _unitOfWork.Save();
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception or print it for debugging purposes
                Console.WriteLine($"Error: {ex.Message}");
                TempData["error"] = "Error creating product";
                return RedirectToAction("Index");
            }

        }

        //edit section deleted here as insert and update are combined

        //delete section was deleted here

        #region API Calls 

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                 return Json(new {success = false,message = "Error while deleting"});
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message="Delete Successfully" });
        }

        #endregion
    }
}
