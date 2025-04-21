using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagmentMVC.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Discount { get; set; } = 0;

        // Foreign keys
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
