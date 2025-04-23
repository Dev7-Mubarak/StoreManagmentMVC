namespace StoreManagmentMVC.ViewModels.DashboardMVs
{
    public class DashboardStats
    {
        public decimal EarningsThisMonth { get; set; }
        public int OrdersThisMonth { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
