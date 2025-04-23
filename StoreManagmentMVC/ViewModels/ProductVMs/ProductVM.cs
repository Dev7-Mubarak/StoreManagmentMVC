namespace StoreManagmentMVC.ViewModels.ProductVMs
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public string StockStatus { get; set; }
    }
}
