using CBTDWeb.Data;
using CBTDWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Categories
{
        public class UpsertModel : PageModel
        {
            private readonly ApplicationDbContext _db;
            [BindProperty]
            public Category objCategory { get; set; }


            public UpsertModel(ApplicationDbContext db)
            {
                _db = db;
            }

            public IActionResult OnGet(int? id)
            {
                objCategory = new Category();

                //edit mode
                if (id != 0)
                {
                    objCategory = _db.Categories.Find(id);
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
                    _db.Categories.Add(objCategory);  // Adds to memory
                    TempData["success"] = "Category added Successfully";
                }
                //if category exists
                else
                {
                    _db.Categories.Update(objCategory);
                    TempData["success"] = "Category updated Successfully";
                }
                _db.SaveChanges();  // Saves to DB

                return RedirectToPage("./Index");
            }
        }
    }



