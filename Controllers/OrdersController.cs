using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce_PetShop.Data;
using E_commerce_PetShop.Models;

namespace E_commerce_PetShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly E_commerce_PetShopContext _context;

        public OrdersController(E_commerce_PetShopContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Order
                .Include(o => o.Product)
                .ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Order
                .Include(o => o.Product)   // ← add this
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var products = _context.Product
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name,
                    // pass price in the text so JS can read it
                }).ToList();

            ViewBag.Products = _context.Product.ToList(); // send full product list
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("OrderId,ProductId,OrderedByName,Quantity,OrderedAt,Status,Total")] Order order)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Product.FindAsync(order.ProductId);
                if (product != null)
                {
                    product.Stock -= order.Quantity;
                    if (product.Stock < 0) product.Stock = 0;
                    _context.Update(product);
                }

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Products = new SelectList(_context.Product, "Id", "Name", order.ProductId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Order.FindAsync(id);
            if (order == null) return NotFound();

            ViewBag.Products = new SelectList(_context.Product, "Id", "Name", order.ProductId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("OrderId,ProductId,OrderedByName,Quantity,OrderedAt,Status,Total")] Order order)
        {
            if (id != order.OrderId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var previousOrder = await _context.Order
                        .AsNoTracking()
                        .FirstOrDefaultAsync(o => o.OrderId == id);

                    var product = await _context.Product.FindAsync(order.ProductId);

                    if (product != null && previousOrder != null)
                    {
                        bool wasCompleted = previousOrder.Status == OrderStatus.Completed;
                        bool nowCompleted = order.Status == OrderStatus.Completed;
                        bool nowCancelled = order.Status == OrderStatus.Cancelled;

                        if (!wasCompleted && nowCompleted)
                        {
                            product.Stock -= order.Quantity;
                            if (product.Stock < 0) product.Stock = 0;
                        }
                        else if (wasCompleted && nowCancelled)
                        {
                            product.Stock += order.Quantity;
                        }

                        _context.Update(product);
                    }
                    {
                        bool wasCompleted = previousOrder.Status == OrderStatus.Completed;
                        bool nowCompleted = order.Status == OrderStatus.Completed;
                        bool nowCancelled = order.Status == OrderStatus.Cancelled;

                        if (!wasCompleted && nowCompleted)
                        {
                            product.Stock -= order.Quantity;
                            if (product.Stock < 0) product.Stock = 0;
                        }
                        else if (wasCompleted && nowCancelled)
                        {
                            product.Stock += order.Quantity;
                        }

                        _context.Update(product);
                    }

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Products = new SelectList(_context.Product, "Id", "Name", order.ProductId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                if (order.Status == OrderStatus.Completed)
                {
                    var product = await _context.Product.FindAsync(order.ProductId);
                    if (product != null)
                    {
                        product.Stock += order.Quantity;
                        _context.Update(product);
                    }
                }

                _context.Order.Remove(order);
            }
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
