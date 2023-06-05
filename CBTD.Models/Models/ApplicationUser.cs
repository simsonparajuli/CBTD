using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTD.ApplicationCore.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[Required]
		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[DisplayName("Street Address")]
		public string StreetAddress { get; set; }

		public string City { get; set; }
		public string State { get; set; }
		[DisplayName("Postal Code")]
		public string PostalCode { get; set; }
		[NotMapped]
		public string FullName { get { return FirstName + " " + LastName; } }

	}
}
