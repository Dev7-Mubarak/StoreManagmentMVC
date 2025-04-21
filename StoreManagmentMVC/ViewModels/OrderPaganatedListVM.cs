namespace StoreManagmentMVC.ViewModels
{
    public class OrderPaganatedListVM
    {
        public List<OrderVM> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int? TotalPages { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string StatusFilter { get; set; }
        public int? CustomerFilter { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public IEnumerable<OrderStatusVM>? OrderStatusVMs { get; set; }
        public IEnumerable<vw_CustomersWithOrders>? AvailableCustomers { get; set; }

    }
}
