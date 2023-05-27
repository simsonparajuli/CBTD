using CBTD.ApplicationCore.Models;
using CBTD.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Categories
{
    public class IndexModel : PageModel
    {
		private readonly UnitOfWork _unitOfWork;  //instance of UnitOfWork
		public IEnumerable<Category> objCategoryList;  //our UI front end will support showing a list of Categories

		public IndexModel(UnitOfWork unitOfWork)  //dependency injection of UnitOfWork service (which includes di for data service) 
		{
            _unitOfWork = unitOfWork;
		}

		public IActionResult OnGet()
		{
			objCategoryList = _unitOfWork.Category.GetAll();
			return Page();
		}

	}
}
