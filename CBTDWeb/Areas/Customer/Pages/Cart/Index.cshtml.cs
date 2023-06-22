using CBTD.ApplicationCore.ViewModels.CBTD.ApplicationCore.ViewModels;
using CBTD.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CBTDWeb.Areas.Customer.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        public ShoppingCartVM ShoppingCartVM { get; set; }
        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OnGet()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                cartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, u => u.ProductId, "Product"),

            };

            foreach (var item in ShoppingCartVM.cartItems)
            {
                item.Price = GetPriceBasedOnQuantity(item.Count, (double)item.Product.UnitPrice,
                    (double)item.Product.HalfDozenPrice, (double)item.Product.DozenPrice);
                ShoppingCartVM.CartTotal += item.Price * item.Count;

            }
            return Page();
        }


        private double GetPriceBasedOnQuantity(double quantity, double price, double priceHalfDozen, double priceDozen)
        {
            if (quantity <= 5)
            {
                return price;
            }
            else
            {
                if (quantity <= 11)
                {
                    return priceHalfDozen;
                }
                return priceDozen;
            }
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _unitOfWork.ShoppingCart.Delete(cart);
            }

            else
            {
                cart.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cart);
            }
            _unitOfWork.Commit();

            var cnt = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).Count();
            //HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            return RedirectToPage();
        }


        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            cart.Count += 1;
            _unitOfWork.ShoppingCart.Update(cart);
            _unitOfWork.Commit();
            return RedirectToPage();
        }


        public IActionResult OnPostRemove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            _unitOfWork.ShoppingCart.Delete(cart);
            _unitOfWork.Commit();
            var cnt = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).Count();
            //HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            return RedirectToPage();


        }

    }
}
