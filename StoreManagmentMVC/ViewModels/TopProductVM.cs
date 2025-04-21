namespace StoreManagmentMVC.ViewModels
{
    public class TopProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int TotalQuantitySold { get; set; }
        public int NumberOfOrders { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TotalRevenue { get; set; }
    }

}
