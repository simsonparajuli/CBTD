using CBTD.ApplicationCore.Models;
using CBTD.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Areas.Admin.Pages.Categories
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public Category objCategory { get; set; }


        public UpsertModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult OnGet(int? id)
        {
            objCategory = new Category();

            //edit mode
            if (id != 0)
            {
                objCategory = _unitOfWork.Category.GetById(id);
            }

            if (objCategory == null)
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
            if (objCategory.Id == 0)
            {
                _unitOfWork.Category.Add(objCategory);  // Adds to memory
                TempData["success"] = "Category added Successfully";
            }
            //if category exists
            else
            {
                _unitOfWork.Category.Update(objCategory);
                TempData["success"] = "Category updated Successfully";
            }
            _unitOfWork.Commit();  // Saves to DB

            return RedirectToPage("./Index");
        }
    }
}



