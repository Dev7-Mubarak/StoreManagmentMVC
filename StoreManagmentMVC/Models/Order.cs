using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagmentMVC.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalAmount { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public bool? IsActive { get; set; } = true;

        // Foreign keys
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
