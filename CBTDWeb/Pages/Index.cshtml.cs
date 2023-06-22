using CBTD.ApplicationCore.Models;
using CBTD.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages
{
    public class IndexModel : PageModel
    {

		public IActionResult OnGet()
        {
            return RedirectToPage("/Home/Index", new { area = "Customer" });

        }
    }
}