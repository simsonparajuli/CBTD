using CBTD.ApplicationCore.Models;
using CBTD.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Areas.Admin.Pages.Manufacturers
{

    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;  //instance of UnitOfWork
        public IEnumerable<Manufacturer> objManufacturerList;  //our UI front end will support showing a list of Categories

        public IndexModel(UnitOfWork unitOfWork)  //dependency injection of UnitOfWork service (which includes di for data service) 
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult OnGet()
        {
            objManufacturerList = _unitOfWork.Manufacturer.GetAll();
            return Page();
        }
    }
}
