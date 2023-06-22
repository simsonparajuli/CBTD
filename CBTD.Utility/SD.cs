using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTD.Utility
{
	public class SD
	{
		// My DB Roles
		public const string AdminRole = "Admin";
		public const string ShipRole = "Shipper";
		public const string CustomerRole = "Customer";



		//for Order Status
		public const string PaymentStatusPending = "Payment Pending";
		public const string PaymentStatusApproved = "Payment Approved";
		public const string StatusApproved = "Approved";
		public const string StatusInProcess = "Processing";
		public const string StatusShipped = "Shipped";
		public const string StatusCancelled = "Cancelled";
		public const string StatusRefunded = "Refunded";

	}
}
