using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer
{
    public class Bill
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public DateTime? Date { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }

}
