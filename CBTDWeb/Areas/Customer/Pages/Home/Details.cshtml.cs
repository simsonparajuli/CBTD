using CBTD.ApplicationCore.Models;
using CBTD.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CBTDWeb.Areas.Customer.Pages.Home
{
  
    public class DetailsModel : PageModel
    {

        private readonly UnitOfWork _unitOfWork;

        public Product objProduct;

        [BindProperty]
        public int txtCount { get; set; }

        public ShoppingCart objCart { get; set; }

        public DetailsModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OnGet(int? productId)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            TempData["UserLoggedIn"] = claim;
            objProduct = _unitOfWork.Product.Get(p => p.Id == productId, includes: "Category,Manufacturer");
            return Page();
        }

        public IActionResult OnPost(Product objProduct)
        {

            //check to see if we have a shopping cart and this item for the user

            var claimsIdentity = User.Identity as ClaimsIdentity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(
            u => u.ApplicationUserId == claim.Value && u.ProductId == objProduct.Id);


            if (cartFromDb == null)
            {
                objCart = new ShoppingCart();
                objCart.ApplicationUserId = claim.Value;
                objCart.ProductId = objProduct.Id;
                objCart.Count = txtCount;
                _unitOfWork.ShoppingCart.Add(objCart);
                _unitOfWork.Commit();

            }

            else
            {

                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, txtCount);
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Commit();
            }
            return RedirectToPage("Index");
        }
    }
}



