using FPT_Book_Store.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FPT_Book_Store.Data
{
    public class ApplicationDbContext : IdentityDbContext<Accounts>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Accounts> Accounts {get; set; }
        public DbSet<Book> Books {get; set; }
        public DbSet<Cart> Carts {get; set; }
        public DbSet<Category> Categories {get; set; }
        public DbSet<Order> Orders {get; set; }
        public DbSet<OrderDetail> OrdersDetail {get; set; }
        public DbSet<Publisher> Publishers {get; set; }
    }
}