using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
                return View("LoginView", model);

            // Hardcoded admin
            if (string.Equals(model.Username, "admin", StringComparison.OrdinalIgnoreCase)
                && model.Password == "admin123")
            {
                HttpContext.Session.SetString("LoggedInUser", "admin");
                HttpContext.Session.SetString("LoggedInName", "Admin");
                return RedirectToAction("Index", "Home");
            }

            // Database user
            var hashed = HashingService.HashPassword(model.Password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == hashed);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View("LoginView", model);
            }

            HttpContext.Session.SetString("LoggedInUser", user.Username);
            HttpContext.Session.SetString("LoggedInName", user.Name);
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View("RegisterView", new Users());
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Users users)
        {
            if (!ModelState.IsValid)
                return View("RegisterView", users);

            bool usernameTaken = await _context.Users.AnyAsync(u => u.Username == users.Username);
            if (usernameTaken)
            {
                ModelState.AddModelError("Username", "Username is already taken.");
                return View("RegisterView", users);
            }

            bool emailTaken = await _context.Users.AnyAsync(u => u.Email == users.Email);
            if (emailTaken)
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View("RegisterView", users);
            }

            users.Password = HashingService.HashPassword(users.Password);
            _context.Add(users);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Account created successfully! Please sign in.";
            return RedirectToAction("Login", "Users");
        }

        // POST: Users/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");
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
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Users users)
        {
            if (!ModelState.IsValid)
            {
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

            // Retrieve user without tracking to avoid accidentally modifying tracked entity
            var users = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (users == null)
            {
                return NotFound();
            }

            // Ensure the form does not display the hashed password
            users.Password = string.Empty;

            return View(users);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Email,Username,Password")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            // If password was left blank on the form, remove password validation so the edit can proceed
            if (string.IsNullOrWhiteSpace(users.Password))
            {
                ModelState.Remove(nameof(users.Password));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Users.FindAsync(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existing.Name = users.Name;
                    existing.Email = users.Email;
                    existing.Username = users.Username;

                    // Only change password when a new one was provided
                    if (!string.IsNullOrWhiteSpace(users.Password))
                    {
                        existing.Password = HashingService.HashPassword(users.Password);
                    }

                    _context.Update(existing);
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