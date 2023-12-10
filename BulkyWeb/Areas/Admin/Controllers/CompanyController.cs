using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;       // to access wwwroot folder
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()              //get all companys
        {
            List<Company> objcompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objcompanyList);
        }

        public IActionResult Upsert(int? id)  // get method         //combining update and insert 
        {
         
            if (id == null || id == 0)
            {
                //create 
                return View(new Company());
            }
            else
            {            //update
                Company companyobj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyobj);
            }
        }


        [HttpPost]
        public IActionResult Upsert(Company companyobj)
        {
            try

            {
               if (ModelState.IsValid)
                { 
             
                    if (companyobj.Id == 0)
                    {
                        _unitOfWork.Company.Add(companyobj);
                    }
                    else
                    {
                        _unitOfWork.Company.Update(companyobj);
                    }

                    _unitOfWork.Save();
                    TempData["success"] = "company created successfully";
                    return RedirectToAction("Index");
                }

                return View(companyobj);
            }
            catch (Exception ex)
            {
                // Log the exception or print it for debugging purposes
                Console.WriteLine($"Error: {ex.Message}");
                TempData["error"] = "Error creating company";
                return RedirectToAction("Index");
            }

        }

        //edit section deleted here as insert and update are combined

        //delete section was deleted here

        #region API Calls 

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objcompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objcompanyList });
        }

       // [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                 return Json(new {success = false,message = "Error while deleting"});
            }
        

            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message="Delete Successfully" });
        }

        #endregion
    }
}
