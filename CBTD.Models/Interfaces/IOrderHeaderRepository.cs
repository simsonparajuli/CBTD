using CBTD.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTD.ApplicationCore.Interfaces
{
    public interface IOrderHeaderRepository<T> : IGenericRepository<OrderHeader>
    {
        void UpdateStatus(int id, string orderStatus, string paymentStatus = null);

		public void UpdateStripePaymentID(int id, string sessionId, string InvoiceId);
	}

}
