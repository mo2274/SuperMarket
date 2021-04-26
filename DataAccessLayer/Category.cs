using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
    public class Category
    {
        [Required]
        public int Id { get; set; }
        [StringLength(40, ErrorMessage = "The value of this field is limited to 40 characters")]
        [Required]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "The value of this field is limited to 200 characters")]
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
