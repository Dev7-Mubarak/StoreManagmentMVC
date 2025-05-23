﻿namespace StoreManagmentMVC.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}
