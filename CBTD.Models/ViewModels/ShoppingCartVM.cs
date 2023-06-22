using CBTD.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTD.ApplicationCore.ViewModels
{
    namespace CBTD.ApplicationCore.ViewModels
    {
        public class ShoppingCartVM
        {
            public Product Product { get; set; }
            [Range(1, 1000, ErrorMessage = "Must be between 1 and 1000")]
            public int Count { get; set; }
            public IEnumerable<ShoppingCart> cartItems { get; set; }

            public double CartTotal { get; set; }

            public OrderHeader OrderHeader { get; set; }
        }
    }

}
