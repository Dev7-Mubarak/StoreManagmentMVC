using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoreManagmentMVC;
using StoreManagmentMVC.ViewModels.ProductVMs;
using System.Data;
namespace StoreManagementSystems.Controllers
{
    public class ProductsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly int _pageSize = 10;

        public ProductsController(ApplicationDbContext context)
            => _context = context;

        // GET: /Products/
        public async Task<ActionResult> Index(
            int page = 1,
            string sort = "Name",
            string sortDir = "ASC",
            int? categoryId = null,
            decimal? priceFrom = null,
            decimal? priceTo = null,
            bool? inStock = null,
            string searchTerm = null)
        {
            // Create parameters for stored procedure
            var parameters = new[]
            {
        new SqlParameter("@PageNumber", page),
        new SqlParameter("@PageSize", _pageSize),
        new SqlParameter("@SortColumn", sort),
        new SqlParameter("@SortDirection", sortDir),
        new SqlParameter("@CategoryFilter", categoryId.HasValue ? (object)categoryId.Value : DBNull.Value),
        new SqlParameter("@PriceFrom", priceFrom.HasValue ? (object)priceFrom.Value : DBNull.Value),
        new SqlParameter("@PriceTo", priceTo.HasValue ? (object)priceTo.Value : DBNull.Value),
        new SqlParameter("@StockFilter", inStock.HasValue ? (object)inStock.Value : DBNull.Value),
        new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? DBNull.Value : (object)searchTerm)
    };

            // Execute stored procedure with multiple result sets
            var connection = _context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "GetPaginatedProducts";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            // Read first result set (products)
            var products = new List<ProductVM>();
            while (await reader.ReadAsync())
            {
                products.Add(new ProductVM
                {
                    ProductId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    StockQuantity = reader.GetInt32(4),
                    CategoryID = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                    CategoryName = reader.IsDBNull(6) ? null : reader.GetString(6),
                    IsActive = reader.GetBoolean(7),
                    StockStatus = reader.GetString(8)
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

            // Get additional data needed for filters/dropdowns
            var categories = await _context.vw_AllCategories.ToListAsync();

            var model = new ProductPaganatedListVM
            {
                Products = products,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)_pageSize),
                SortColumn = sort,
                SortDirection = sortDir,
                CategoryFilter = categoryId,
                PriceFrom = priceFrom,
                PriceTo = priceTo,
                InStockFilter = inStock,
                SearchTerm = searchTerm,
                AvailableCategories = categories
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.vw_AllCategories.ToListAsync();
            var viewModel = new CreateProductVM
            {
                AvailableCategories = categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.Name
                    }).ToList()
            };

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductVM model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _context.vw_AllCategories.ToListAsync();
                model.AvailableCategories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList();
                return View(model);
            }

            var productIdParam = new SqlParameter
            {
                ParameterName = "@ProductId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Product_Create @Name, @Description, @Price, @StockQuantity, @CategoryId, @IsActive",
                new SqlParameter("@Name", model.Name),
                new SqlParameter("@Description", model.Description ?? (object)DBNull.Value),
                new SqlParameter("@Price", model.Price),
                new SqlParameter("@StockQuantity", model.StockQuantity),
                new SqlParameter("@CategoryId", model.CategoryId),
                new SqlParameter("@IsActive", model.IsActive)
            );

            return RedirectToAction("Index");
        }

        // GET: /Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = _context.Products
                   .FromSqlRaw("EXEC sp_GetProductById @ProductId", new SqlParameter("@ProductId", id))
                   .AsEnumerable()
                   .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            var categories = await _context.vw_AllCategories.ToListAsync();

            var viewModel = new CreateProductVM
            {
                ProductId = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryID,
                IsActive = product.IsActive,
                AvailableCategories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: /Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateProductVM model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _context.vw_AllCategories.ToListAsync();
                model.AvailableCategories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList();
                return View(model);
            }

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Product_Update @ProductId, @Name, @Description, @Price, @StockQuantity, @CategoryId, @IsActive",
                new SqlParameter("@ProductId", model.ProductId),
                new SqlParameter("@Name", model.Name),
                new SqlParameter("@Description", model.Description ?? (object)DBNull.Value),
                new SqlParameter("@Price", model.Price),
                new SqlParameter("@StockQuantity", model.StockQuantity),
                new SqlParameter("@CategoryId", model.CategoryId),
                new SqlParameter("@IsActive", model.IsActive)
            );

            return RedirectToAction("Index");
        }


        // GET: /Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC sp_DeleteProduct @ProductID={id}"
            );
            return RedirectToAction(nameof(Index));
        }
    }

}