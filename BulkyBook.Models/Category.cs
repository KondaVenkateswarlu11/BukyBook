using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 1000, ErrorMessage = "Display Order Must be in between 1 and 1000!!")]
        public int DisplayOrder { get; set; }

        //Setting the Default time 
        public DateTime CreateddateTime { get; set; } = DateTime.Now;
    }
}
