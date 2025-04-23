namespace StoreManagmentMVC.ViewModels.OrderVMs
{
    public class OrderVM
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public int ItemCount { get; set; }
    }
}
