using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StoreManagmentMVC.ViewModels.ProductVMs
{
    public class CreateProductVM
    {
        public int ProductId { get; set; } // For edit

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999,999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be 0 or more")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public List<SelectListItem>? AvailableCategories { get; set; }
    }
}
