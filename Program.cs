using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using E_commerce_PetShop.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<E_commerce_PetShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("E_commerce_PetShopContext") ?? throw new InvalidOperationException("Connection string 'E_commerce_PetShopContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route -> UsersController.Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Login}/{id?}");

app.Run();
