using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Supermarket : DbContext
    {
        public Supermarket() : base("name=SuperMarket")
        {

        }

        public virtual DbSet<Product> Products { set; get; }
        public virtual DbSet<Category> Categories { set; get; }
        public virtual DbSet<Seller> Sellers { set; get; }
        public virtual DbSet<Item> Items { set; get; }
        public virtual DbSet<Bill> Bills { set; get; }

    }


}
