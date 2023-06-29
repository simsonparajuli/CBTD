using CBTD.ApplicationCore.Models;
using CBTD.ApplicationCore.ViewModels;
using CBTD.DataAccess;
using CBTD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CBTDWeb.Areas.Admin.Controllers
{

	namespace CBTDWeb.Areas.Admin.Controllers
	{
		[Area("Admin")]
		[Authorize]
		public class OrdersController : Controller
		{
			private readonly UnitOfWork _unitOfWork;
			[BindProperty]
			public OrderVM OrderVM { get; set; }
			public OrdersController(UnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public IActionResult Index()
			{
				return View();
			}

			public IActionResult Details(int orderId)
			{
				OrderVM = new OrderVM()
				{
					OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includes: "ApplicationUser"),
					OrderDetail = _unitOfWork.OrderDetails.GetAll(u => u.OrderId == orderId, includes: "Product"),
				};
				return View(OrderVM);
			}

			// Update Order Detail(Post)
			[HttpPost]
			[Authorize(Roles = SD.AdminRole + "," + SD.ShipRole)]
			[ValidateAntiForgeryToken]
			public IActionResult UpdateOrderDetail()
			{
				var orderHEaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id, asNotTracking: false);
				orderHEaderFromDb.Name = OrderVM.OrderHeader.Name;
				orderHEaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
				orderHEaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
				orderHEaderFromDb.City = OrderVM.OrderHeader.City;
				orderHEaderFromDb.State = OrderVM.OrderHeader.State;
				orderHEaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
				if (OrderVM.OrderHeader.Carrier != null)
				{
					orderHEaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
				}
				if (OrderVM.OrderHeader.TrackingNumber != null)
				{
					orderHEaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
				}
				_unitOfWork.OrderHeader.Update(orderHEaderFromDb);
				_unitOfWork.Commit();
				TempData["Success"] = "Order Details Updated Successfully.";
				return RedirectToAction("Details", "Orders", new { orderId = orderHEaderFromDb.Id });
			}

			// Ship Order(Post)
			[HttpPost]
			[Authorize(Roles = SD.AdminRole + "," + SD.ShipRole)]
			[ValidateAntiForgeryToken]
			public IActionResult ShipOrder()
			{
				var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id, asNotTracking: false);
				orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
				orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
				orderHeader.OrderStatus = SD.StatusShipped;
				orderHeader.ShippingDate = DateTime.Now;

				_unitOfWork.OrderHeader.Update(orderHeader);
				_unitOfWork.Commit();
				TempData["Success"] = "Order Shipped Successfully.";
				return RedirectToAction("Details", "Orders", new { orderId = OrderVM.OrderHeader.Id });
			}

			// Cancel Order (Post)
			[HttpPost]
			[Authorize(Roles = SD.AdminRole + "," + SD.ShipRole)]
			[ValidateAntiForgeryToken]
			public IActionResult CancelOrder()
			{
				var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id, asNotTracking: false);

				//Stripe Refund logic would go here

				_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);

				_unitOfWork.Commit();

				TempData["Success"] = "Order Canceled Successfully.";
				return RedirectToAction("Details", "Orders", new { orderId = OrderVM.OrderHeader.Id });
			}


			#region API CALLS
			[HttpGet]
			public IActionResult GetAll(string status)
			{
				IEnumerable<OrderHeader> orderHeaders;
				orderHeaders = _unitOfWork.OrderHeader.GetAll(includes: "ApplicationUser");

				if (User.IsInRole(SD.AdminRole) || User.IsInRole(SD.ShipRole))
				{
					orderHeaders = _unitOfWork.OrderHeader.GetAll(includes: "ApplicationUser");
				}
				else  //just get the orders for the Customer who is logged in
				{
					var claimsIdentity = (ClaimsIdentity)User.Identity;
					var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
					orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == claim.Value, includes: "ApplicationUser");
				}

				switch (status) //apply where filters depending on which tab the user selected
				{
					case "inprocess":
						orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
						break;
					case "pending":
						orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusPending);
						break;
					case "shipped":
						orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
						break;
					case "canceled":
						orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusCancelled);
						break;
					default:
						break;
				}

				return Json(new { data = orderHeaders });   //this is what table tables needs a JSON string 
			}

			#endregion

		}
	}
}