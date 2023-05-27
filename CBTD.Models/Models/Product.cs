using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTD.ApplicationCore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "MSRP List Price")]
        [Range(0.01, 10000)]
        public decimal ListPrice { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [Display(Name = "Price 1-5")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [Display(Name = "Price 6-11")]
        public decimal HalfDozenPrice { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [Display(Name = "Price 12 or more")]
        public decimal DozenPrice { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public string UPC { get; set; }

        public string ImageUrl { get; set; }

        [Required]

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }

        /// <summary>
        /// These are the two other objects I'm relating, linking the PK to the FK
        /// </summary>

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ForeignKey("ManufacturerId")]
        public Manufacturer Manufacturer { get; set; }
    }
}
