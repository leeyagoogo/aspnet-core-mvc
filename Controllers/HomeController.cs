using System.Diagnostics;
using E_commerce_PetShop.Models;
using E_commerce_PetShop.Data;
using E_commerce_PetShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_PetShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly E_commerce_PetShopContext _context;

        public HomeController(ILogger<HomeController> logger, E_commerce_PetShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var vm = new DashboardViewModel
            {
                TotalOrders = await _context.Order.CountAsync(),
                TotalSalesToday = await _context.Order
    .Where(o => o.Status == OrderStatus.Completed)
    .SumAsync(o => (decimal?)o.Total) ?? 0,
                TotalCustomers = await _context.Users.CountAsync(),
                ProductsInStock = await _context.Product.SumAsync(p => p.Stock),
                ProductsCount = await _context.Product.CountAsync(),
                LowStockAlerts = await _context.Product.CountAsync(p => p.Stock <= 5),
                LowStockProducts = await _context.Product
                       .Where(p => p.Stock <= 5)
                       .OrderBy(p => p.Stock)
                       .ToListAsync(),
                RecentOrders = await _context.Order
                                       .Include(o => o.Product)
                                       .OrderByDescending(o => o.OrderedAt)
                                       .Take(5)
                                       .ToListAsync()
            };

            return View(vm);
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
