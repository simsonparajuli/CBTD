using CBTD.ApplicationCore.Models;
using CBTD.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Products
{
    public class IndexModel : PageModel
    {

        private readonly UnitOfWork _unitOfWork;

        public IEnumerable<Product> objProductList;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OnGet()
        {
            objProductList = _unitOfWork.Product.GetAll();
            return Page();
        }
    }
}
