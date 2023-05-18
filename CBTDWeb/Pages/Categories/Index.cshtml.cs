using CBTDWeb.Data;
using CBTDWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Categories
{
    public class IndexModel : PageModel
    {
		private readonly ApplicationDbContext _db;  //instance of database service
		public List<Category> objCategoryList;  //our UI front end will support showing a list of Categories

		public IndexModel(ApplicationDbContext db)  //dependency injection of database service
		{
			_db = db;
		}

		public IActionResult OnGet()
		{
			objCategoryList = _db.Categories.ToList();
			return Page();
		}

	}
}
