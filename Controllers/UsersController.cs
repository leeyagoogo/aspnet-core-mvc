using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce_PetShop.Data;
using E_commerce_PetShop.Models;
using E_commerce_PetShop.Services;
using E_commerce_PetShop.ViewModel;

namespace E_commerce_PetShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly E_commerce_PetShopContext _context;

        public UsersController(E_commerce_PetShopContext context)
        {
            _context = context;
        }

        // GET: Users/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View("LoginView", new LoginViewModel());
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("LoginView", model);
            }

            // Alternate fixed credentials
            if (string.Equals(model.Username, "admin", StringComparison.OrdinalIgnoreCase)
                && model.Password == "admin123")
            {
                TempData["LoggedInUser"] = "admin";
                return RedirectToAction("Index", "Home");
            }

            // Check scaffolded users table (passwords are stored hashed)
            var hashed = HashingService.HashPassword(model.Password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == hashed);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View("LoginView", model);
            }

            // Successful login (replace with proper auth in production)
            TempData["LoggedInUser"] = user.Username;
            return RedirectToAction("Index", "Home");
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewBag.UserRole = new SelectList(new[]
            {
                new { RoleId = 1, UserRole = "Admin" },
                new { RoleId = 2, UserRole = "User" }
            }, "RoleId", "UserRole");

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Users users)
        {
            if (!ModelState.IsValid)
            {
                // repopulate ViewBag before returning view on validation errors
                ViewBag.UserRole = new SelectList(new[]
                {
                    new { RoleId = 1, UserRole = "Admin" },
                    new { RoleId = 2, UserRole = "User" }
                }, "RoleId", "UserRole");

                return View(users);
            }

            string HashPassword = HashingService.HashPassword(users.Password);
            users.Password = HashPassword;

            _context.Add(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["Role"] = new SelectList(_context.Role, "RoleId", "UserRole");
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Email,Username,Password")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
