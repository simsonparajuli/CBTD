using CBTD.ApplicationCore.Models;
using CBTD.ApplicationCore.ViewModels.CBTD.ApplicationCore.ViewModels;
using CBTD.DataAccess;
using CBTD.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using System.Security.Claims;

namespace CBTDWeb.Areas.Customer.Pages.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public SummaryModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                cartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includes: "Product"),
                OrderHeader = new()
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(
                u => u.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.FullName;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartVM.cartItems)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, (double)cart.Product.UnitPrice,
                    (double)cart.Product.HalfDozenPrice, (double)cart.Product.DozenPrice);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;

            }
            return Page();
        }

        private double GetPriceBasedOnQuantity(int quantity, double price, double priceHalfDozen, double priceDozen)
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

        public IActionResult OnPost()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //The Order is actually the shopping cart items, so we might as well use it

            ShoppingCartVM.cartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                    includes: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

            foreach (var cart in ShoppingCartVM.cartItems)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, (double)cart.Product.UnitPrice,
                (double)cart.Product.HalfDozenPrice, (double)cart.Product.DozenPrice);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == claim.Value);

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusInProcess;

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Commit();

            //Once the Order Header is created, we can start cycling through our Order Details

            foreach (var cart in ShoppingCartVM.cartItems)
            {
                OrderDetails orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetails.Add(orderDetail);
                _unitOfWork.Commit();
            }

            //don't forget to clear the physical shopping cart entries

            _unitOfWork.ShoppingCart.Delete(ShoppingCartVM.cartItems);
            _unitOfWork.Commit();

            //stripe settings 
            var domain = "https://localhost:7025/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"cart/OrderConfirmation?Orderid={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"cart/index",
            };

            foreach (var item in ShoppingCartVM.cartItems)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),//20.00 -> 2000
                        Currency = "usd",

                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name

                        },

                    },
                    Quantity = item.Count,



                };

                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);

            //grab the URL from the session
            Response.Headers.Add("Location", session.Url);
            //return a redirect to the original session location
            _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentLinkId);
            _unitOfWork.Commit();

            return new StatusCodeResult(303);  //success URL

        }

    }

}
