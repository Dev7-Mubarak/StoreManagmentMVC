using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoreManagmentMVC.Models;
using StoreManagmentMVC.ViewModels.DashboardMVs;
using System.Diagnostics;

namespace StoreManagmentMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Now.AddMonths(-1);
            endDate ??= DateTime.Now;

            var salesData = _context.Set<SalesSummary>()
                .FromSqlRaw("EXEC GetSalesSummaryByDate @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsNoTracking()
                .ToList();

            var param = new SqlParameter("@Top", 5);

            var topProducts = await _context.TopSellingProducts
                .FromSqlRaw("SELECT * FROM fn_TopSellingProducts(@Top)", param)
                .ToListAsync();

            var stats = _context.EcommerceDashboardStats.FirstOrDefault();

            var model = new DashboardVM
            {
                Sales = salesData,
                TopProducts = topProducts,
                EarningsThisMonth = stats.EarningsThisMonth,
                TotalProducts = stats.TotalProducts,
                AverageOrderValue = stats.AverageOrderValue,
                OrdersThisMonth = stats.OrdersThisMonth,
                TotalCustomers = stats.TotalCustomers,
                TotalOrders = stats.TotalOrders
            };


            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
