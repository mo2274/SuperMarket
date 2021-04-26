using System;


namespace DataAccessLayer
{
    public class BillData
    {
        public int BillId { get; set; }
        public string SellerName { get; set; }
        public DateTime? BillDate { get; set; }
        public int Total { get; set; }
    }
}
