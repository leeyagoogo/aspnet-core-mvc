using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using E_commerce_PetShop.Models;

namespace E_commerce_PetShop.Data
{
    public class E_commerce_PetShopContext : DbContext
    {
        public E_commerce_PetShopContext (DbContextOptions<E_commerce_PetShopContext> options)
            : base(options)
        {
        }

        public DbSet<E_commerce_PetShop.Models.Users> Users { get; set; } = default!;

        public DbSet<E_commerce_PetShop.Models.Product> Product { get; set; } = default!;
        public DbSet<E_commerce_PetShop.Models.Order> Order { get; set; } = default!;

    }
}
