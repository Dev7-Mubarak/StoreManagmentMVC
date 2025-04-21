using System.ComponentModel.DataAnnotations;

namespace StoreManagmentMVC.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public bool? IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Order> Orders { get; set; }
    }

}
