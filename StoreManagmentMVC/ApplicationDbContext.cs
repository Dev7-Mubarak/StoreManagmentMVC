using Microsoft.EntityFrameworkCore;
using StoreManagmentMVC.Models;
using StoreManagmentMVC.ViewModels;

namespace StoreManagmentMVC
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Sale> DailySalesData { get; set; }
        public DbSet<TopProductVM> TopProductVM { get; set; }
        public DbSet<OrderDetailVM> OrderDetailVMs { get; set; }
        public DbSet<OrderStatusVM> ActiveOrderStatuses { get; set; }
        public virtual DbSet<vw_CustomersWithOrders> vw_CustomersWithOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>().HasNoKey();
            modelBuilder.Entity<TopProductVM>().HasNoKey();
            modelBuilder.Entity<OrderDetailVM>().HasNoKey();
            modelBuilder.Entity<OrderVM>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<OrderStatusVM>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_OrderStatuses");
            });
            modelBuilder.Entity<vw_CustomersWithOrders>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_CustomersWithOrders");
            });

        }


    }
}
