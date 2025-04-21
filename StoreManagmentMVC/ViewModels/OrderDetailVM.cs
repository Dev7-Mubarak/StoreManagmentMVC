namespace StoreManagmentMVC.ViewModels
{
    public class OrderDetailVM
    {
        public int orderId { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
    }
}
