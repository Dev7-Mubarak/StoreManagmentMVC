using Microsoft.EntityFrameworkCore;
using StoreManagmentMVC.Models;
using StoreManagmentMVC.ViewModels;
using StoreManagmentMVC.ViewModels.DashboardMVs;
using StoreManagmentMVC.ViewModels.OrderVMs;
using StoreManagmentMVC.ViewModels.ProductVMs;

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

        public DbSet<SalesSummary> DailySalesData { get; set; }
        public DbSet<OrderDetailVM> OrderDetailVMs { get; set; }
        public DbSet<OrderStatusVM> ActiveOrderStatuses { get; set; }
        public virtual DbSet<vw_CustomersWithOrders> vw_CustomersWithOrders { get; set; }
        public virtual DbSet<vw_AllCategories> vw_AllCategories { get; set; }
        public virtual DbSet<DashboardStats> EcommerceDashboardStats { get; set; }
        public DbSet<TopSellingProductVM> TopSellingProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesSummary>().HasNoKey();
            modelBuilder.Entity<OrderDetailVM>().HasNoKey();
            modelBuilder.Entity<TopSellingProductVM>().HasNoKey();

            modelBuilder.Entity<OrderVM>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<ProductVM>(entity =>
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
            modelBuilder.Entity<vw_AllCategories>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_AllCategories");
            });

            modelBuilder.Entity<DashboardStats>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_EcommerceDashboardStats");
            });

        }


    }
}
