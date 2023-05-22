using CBTD.DataAccess;
using CBTD.Models;
using CBTD.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Manufacturers
{
        public class UpsertModel : PageModel
        {
            private readonly IUnitOfWork _unitOfWork;
            [BindProperty]
            public Manufacturer objManufacturer { get; set; }


            public UpsertModel(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public IActionResult OnGet(int? id)
            {
                objManufacturer = new Manufacturer();

                //edit mode
                if (id != 0)
                {
                    objManufacturer = _unitOfWork.Manufacturer.GetById(id);
                }

                if (objManufacturer == null)
                {
                    return NotFound();
                }

                //create new mode
                return Page();
            }

            public IActionResult OnPost()
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                //if this is a new category
                if (objManufacturer.Id == 0)
                {
                    _unitOfWork.Manufacturer.Add(objManufacturer);  // Adds to memory
                    TempData["success"] = "Manufacturer added Successfully";
                }
                //if category exists
                else
                {
                    _unitOfWork.Manufacturer.Update(objManufacturer);
                    TempData["success"] = "Manufacturer updated Successfully";
                }
                _unitOfWork.Commit();  // Saves to DB

                return RedirectToPage("./Index");
            }
        }
    }



