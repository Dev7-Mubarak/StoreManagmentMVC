using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoreManagmentMVC.Models;
using StoreManagmentMVC.ViewModels;
using System.Data;

namespace StoreManagmentMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int _pageSize = 10;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrdersController
        public async Task<ActionResult> Index(
            int page = 1,
            string sort = "OrderDate",
            string sortDir = "DESC",
            string status = null,
            int? customerId = null,
            string dateFrom = null,
            string dateTo = null)
        {

            DateTime? parsedDateFrom = ParseDate(dateFrom);
            DateTime? parsedDateTo = ParseDate(dateTo);

            // Create parameters for stored procedure
            var parameters = new[]
            {
            new SqlParameter("@PageNumber", page),
            new SqlParameter("@PageSize", _pageSize),
            new SqlParameter("@SortColumn", sort),
            new SqlParameter("@SortDirection", sortDir),
            new SqlParameter("@StatusFilter", string.IsNullOrEmpty(status) ? DBNull.Value : (object)status),
            new SqlParameter("@CustomerId", customerId.HasValue ? (object)customerId.Value : DBNull.Value),
            new SqlParameter("@DateFrom", parsedDateFrom.HasValue ? (object)parsedDateFrom.Value : DBNull.Value),
            new SqlParameter("@DateTo", parsedDateTo.HasValue ? (object)parsedDateTo.Value : DBNull.Value)
        };

            // Execute stored procedure with multiple result sets
            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "GetPaginatedOrders";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            // Read first result set (orders)
            var orders = new List<OrderVM>();
            while (await reader.ReadAsync())
            {
                orders.Add(new OrderVM
                {
                    OrderId = reader.GetInt32(0),
                    OrderDate = reader.GetDateTime(1),
                    CustomerId = reader.GetInt32(2),
                    CustomerName = reader.GetString(3),
                    TotalAmount = reader.GetDecimal(4),
                    Status = reader.GetString(5),
                    ItemCount = reader.GetInt32(6)
                });
            }

            // Read second result set (total count)
            int totalCount = 0;
            if (await reader.NextResultAsync())
            {
                if (await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }
            }

            await connection.CloseAsync();


            var activeOrderStatuse = await _context.ActiveOrderStatuses.ToListAsync();
            var availableCustomers = await _context.vw_CustomersWithOrders.ToListAsync();

            var model = new OrderPaganatedListVM
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)_pageSize),
                SortColumn = sort,
                SortDirection = sortDir,
                StatusFilter = status,
                CustomerFilter = customerId,
                DateFrom = dateFrom,
                DateTo = dateTo,
                OrderStatusVMs = activeOrderStatuse,
                AvailableCustomers = availableCustomers
            };


            return View(model);
        }

        public async Task<ActionResult> GetOrderDetails(int orderId)
        {
            var orderDetails = await _context.OrderDetailVMs
                .FromSqlRaw("EXEC GetOrderDetailsById @OrderId",
                    new SqlParameter("@OrderId", orderId))
                .ToListAsync();

            return Json(orderDetails);
        }
        private DateTime? ParseDate(string dateString)
        {
            if (DateTime.TryParse(dateString, out var date))
            {
                return date;
            }
            return null;
        }


        // GET: OrdersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrdersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            return View();
        }

        // GET: OrdersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
