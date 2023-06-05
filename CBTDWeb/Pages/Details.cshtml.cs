using CBTD.ApplicationCore.Models;
using CBTD.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages
{
    public class DetailsModel : PageModel
    {

		private readonly UnitOfWork _unitOfWork;


		public Product objProduct;

		public DetailsModel(UnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult OnGet(int? productId)
        {
			objProduct = _unitOfWork.Product.Get(p => p.Id == productId, includes:"Category,Manufacturer");
			return Page();
        }
    }
}
