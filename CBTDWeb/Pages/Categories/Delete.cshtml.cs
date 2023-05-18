using CBTDWeb.Data;
using CBTDWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category objCategory { get; set; }


        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult OnGet(int? id)
        {
            objCategory = new Category();

            objCategory = _db.Categories.Find(id);


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


            _db.Categories.Remove(objCategory);  // Removes to memory
            TempData["success"] = "Category Deleted Successfully";

            _db.SaveChanges();  // Saves to DB

            return RedirectToPage("./Index");
        }
    }
}



