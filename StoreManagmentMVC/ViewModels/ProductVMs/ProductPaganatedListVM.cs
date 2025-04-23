namespace StoreManagmentMVC.ViewModels.ProductVMs
{
    public class ProductPaganatedListVM
    {
        public List<ProductVM> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int? CategoryFilter { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public bool? InStockFilter { get; set; }
        public string SearchTerm { get; set; }
        public List<vw_AllCategories> AvailableCategories { get; set; }
    }
}
