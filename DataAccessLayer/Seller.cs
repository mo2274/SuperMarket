using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
    public class Seller
    {
        [Required]
        public int Id { get; set; }
        [StringLength(40, ErrorMessage = "The value of this field is limited to 40 characters")]
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(18, 100)]
        public int Age { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "The value of this field is limited to 20 characters")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "The value of this field is limited to 20 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "The value of this field is limited to 40 characters")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
