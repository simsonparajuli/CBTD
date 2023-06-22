using CBTD.ApplicationCore.Models;
using CBTD.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Areas.Admin.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public Category objCategory { get; set; }


        public DeleteModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult OnGet(int? id)
        {
            objCategory = new Category();

            objCategory = _unitOfWork.Category.GetById(id);


            if (objCategory == null)
            {
                return NotFound();
            }

            //REfresh page
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            _unitOfWork.Category.Delete(objCategory);  // Removes to memory
            TempData["success"] = "Category Deleted Successfully";

            _unitOfWork.Commit();  // Saves to DB

            return RedirectToPage("./Index");
        }
    }
}



