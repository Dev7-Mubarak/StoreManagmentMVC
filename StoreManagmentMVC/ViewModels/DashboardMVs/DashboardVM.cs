namespace StoreManagmentMVC.ViewModels.DashboardMVs
{
    public class DashboardVM
    {
        public List<SalesSummary>? Sales { get; set; }
        public List<TopSellingProductVM>? TopProducts { get; set; }
        public decimal EarningsThisMonth { get; set; }
        public int OrdersThisMonth { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
