using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;


namespace DataAccessLayer
{
    public class DataBaseAccess
    {

        #region Product Methods

        public static List<DisplayType> GetProducts()
        {
            using (var db = new Supermarket())
            {
                return db.Products.Include("Category")
                    .Select(p => new DisplayType() { Id = p.Id, Name = p.Name, Quantity = p.Quantity, Price = p.Price, Category = p.Category.Name })
                    .ToList();
            }
        }

        public static List<DisplayType> GetProductsByCategory(string CategoryName)
        {
            using (var db = new Supermarket())
            {
                return db.Products.Include("Category").Where(p => p.Category.Name == CategoryName)
                    .Select(p => new DisplayType() { Id = p.Id, Name = p.Name, Quantity = p.Quantity, Price = p.Price, Category = p.Category.Name })
                    .ToList();
            }
        }

        public static bool AddProduct(string name, int quantity, double price, Category category)
        {
            try
            {
                using (var db = new Supermarket())
                {
                    var product = new Product()
                    {
                        Name = name,
                        Quantity = quantity,
                        Price = price,
                        CategoryId = category.Id,
                    };
                    db.Products.Add(product);
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool EditProduct(int id, string name, int quantity, double price, Category category)
        {
            try
            {
                using (var db = new Supermarket())
                {
                    var product = db.Products.Find(id);
                    if (product == null)
                        return false;
                    product.Name = name;
                    product.Quantity = quantity;
                    product.Price = price;
                    product.CategoryId = category.Id;                   
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool DeleteProduct(int id)
        {
            try
            {
                using (var db = new Supermarket())
                {
                    var product = db.Products.Find(id);
                    if (product == null)
                        return false;
                    db.Products.Remove(product);
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static Product GetProductByName(string productName)
        {
            using (var db = new Supermarket())
            {
                return db.Products.FirstOrDefault(p => p.Name == productName);           
            }
        }

        #endregion
        #region Seller Methods
        public static bool EditSeller(int id, string name, int age, string phone, string password, string emailAddress)
        {
            try
            {
                using (var db = new Supermarket())
                {
                    var seller = db.Sellers.Find(id);
                    if (seller == null)
                        return false;
                    seller.Name = name;
                    seller.Age = age;
                    seller.Phone = phone;
                    seller.Password = password;
                    seller.EmailAddress = emailAddress;
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool DeleteSeller(int id)
        {
            try
            {
                using (var db = new Supermarket())
                {
                    var seller = db.Sellers.Find(id);
                    if (seller == null)
                        return false;
                    db.Sellers.Remove(seller);
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static Seller CheckIfSellerExists(string userName, string password)
        {
            using (var db = new Supermarket())
            {
                var sellerData = db.Sellers.FirstOrDefault(s => s.Name == userName);
                if (sellerData == null)
                    return null;
                if (sellerData.Password != password)
                    return null;

                return sellerData;
            }
            
        }

        public static List<Seller> GetSellers()
        {
            using (var db = new Supermarket())
            {
                return db.Sellers.ToList();
            }
        }

        public static bool AddSeller(string name, int age, string phone, string password, string emailAddress)
        {
            try
            {
                using (var db = new Supermarket())
                {
                    var seller = new Seller()
                    {
                        Name = name,
                        Age = age,
                        Phone = phone,
                        Password = password,
                        EmailAddress = emailAddress
                    };
                    db.Sellers.Add(seller);
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region Category Methods
        public static Category GetCategoryByName(string categoryName)
        {
            using (var db = new Supermarket())
            {
                var category = db.Categories.Where(c => c.Name == categoryName).First();
                return category;
            }
        }

        public static List<CategoryDisplay> GetCategories()
        {
            using (var db = new Supermarket())
            {
                return db.Categories.Select(c => new CategoryDisplay{ Id = c.Id, Name = c.Name, Description = c.Description}).ToList();
            }
        }

        public static bool AddCategory(string name, string description)
        {
            try
            {
                using (var db = new Supermarket())
                {
                    var category = new Category() { Name = name, Description = description};
                    db.Categories.Add(category);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool EditCategory(int id, string name, string description)
        {
            try
            {
                using (var db = new Supermarket())
                {
                    var category = db.Categories.Find(id);
                    if (category == null)
                        return false;
                    category.Name = name;
                    category.Description = description;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool DeleteCategory(int id)
        {
            try
            {
                using (var db = new Supermarket())
                {
                    var category = db.Categories.Find(id);
                    if (category == null)
                        return false;
                    db.Categories.Remove(category);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion
        #region Bill Methods
        public static bool IsBillExist(int billId)
        {
            using (var db = new Supermarket())
            {
                var bill = db.Bills.Find(billId);
                if (bill == null)
                    return false;
            }
            return true;
        }

        public static void AddItemToExistingBill(int billId, string productName, int quantiy)
        {
            using (var db = new Supermarket())
            {
                var bill = db.Bills.Find(billId);
                if (bill == null)
                    return;
                var item = new Item()
                {
                    BillId = bill.Id,
                    ProductId = GetProductByName(productName).Id,
                    ProductQuantity = quantiy
                };
                db.Items.Add(item);
                db.SaveChanges();
            }
        }

        private static void AddBill(int billId, Seller seller)
        {
            using (var db = new Supermarket())
            {
                var bill = new Bill()
                {
                    Id = billId,
                    SellerId = seller.Id,
                    Date = DateTime.Now
                };
                db.Bills.Add(bill);
                db.SaveChanges();
            }
        }
        
        public static List<BillData> GetBills()
        {
            using (var db = new Supermarket())
            {
                var bills = db.Bills.Include("Seller")
                    .Select(b => new BillData() { BillId = b.Id, SellerName = b.Seller.Name, BillDate = b.Date })
                    .ToList();
                foreach (var bill in bills)
                {
                    bill.Total = CalculateTotal(bill.BillId);
                }

                return bills;
            }
        }
        public static Bill GetBill(int billId)
        {
            var db = new Supermarket();
            return db.Bills.Include(b => b.Items).First(b => b.Id == billId);               
        }
        #endregion
        #region Items Methods
        public static bool AddItem(int billId, string productName, int quantiy, Seller seller)
        {
            try
            {
                using (var db = new Supermarket())
                {
                    var bill = db.Bills.Find(billId);
                    if (bill == null)
                        AddBill(billId, seller);
                    var product = db.Products.FirstOrDefault(p => p.Name == productName);
                    var item = new Item()
                    {
                        BillId = billId,
                        ProductId = product.Id,
                        ProductQuantity = quantiy
                    };
                    db.Items.Add(item);
                    product.Quantity -= quantiy;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
           
           
        }

        public static List<Item> GetItems(int billId)
        {
            using (var db = new Supermarket())
            {
                return db.Items.Include("Product").Where(i => i.BillId == billId)
                    .ToList();
            }

        }

        public static int CalculateTotal(int billId)
        {
            var items = GetItems(billId);
            return (int)items.Select(i => i.Product.Price * i.ProductQuantity).Sum();
        }

        #endregion
    }
}
