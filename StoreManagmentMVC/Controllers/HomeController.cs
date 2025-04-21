using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreManagmentMVC.Models;
using StoreManagmentMVC.ViewModels;
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

        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Now.AddMonths(-1);
            endDate ??= DateTime.Now;

            var salesData = _context.Set<Sale>()
                .FromSqlRaw("EXEC sp_GetSalesReport @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsNoTracking()
                .ToList();

            var topProducts = _context.TopProductVM
                .FromSqlRaw("EXEC sp_GetTopProductsByQuantity @TopN = {0}, @StartDate = {1}, @EndDate = {2}, @CategoryId = {3}",
                    5,
                    new DateTime(2023, 04, 01),
                    DateTime.Now,
                    DBNull.Value
                )
                .ToList();
            var model = new DashboardVM
            {
                Sales = salesData,
                TopProducts = topProducts
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
