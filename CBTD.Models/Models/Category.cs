using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CBTD.ApplicationCore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100 only!!")]
        public int DisplayOrder { get; set; }

        public DateTime DateModified { get; set; } = DateTime.Now;

    }
}
