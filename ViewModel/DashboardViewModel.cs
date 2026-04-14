using System;
using System.Collections.Generic;
using E_commerce_PetShop.Models;

namespace E_commerce_PetShop.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalSalesToday { get; set; }
        public decimal TotalSalesThisMonth { get; set; }
        public int TotalCustomers { get; set; }
        public int ProductsInStock { get; set; }
        public int ProductsCount { get; set; }
        public int LowStockAlerts { get; set; }
        public List<Product> LowStockProducts { get; set; } = new();
        // ADD THIS ?
        public List<Order> RecentOrders { get; set; } = new();
    }
}
