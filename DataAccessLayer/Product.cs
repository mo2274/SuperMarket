using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [StringLength(40, ErrorMessage = "The value of this field is limited to 40 characters")]
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<Item> Items { get; set; }
    }

}
