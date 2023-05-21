using CBTD.DataAccess;
using CBTD.Models;
using CBTD.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Categories
{
    public class IndexModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;  //instance of UnitOfWork
		public IEnumerable<Category> objCategoryList;  //our UI front end will support showing a list of Categories

		public IndexModel(IUnitOfWork unitOfWork)  //dependency injection of UnitOfWork service (which includes di for data service) 
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult OnGet()
		{
			objCategoryList = _unitOfWork.Category.ToList();
			return Page();
		}

	}
}
