using Microsoft.EntityFrameworkCore;
using WebApplicationtodolist.Models;
using Microsoft.Extensions.Configuration;

namespace WebApplicationtodolist.Data
{
    
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
            public DbSet<TodoItem> TodoItems { get; set; }
        }
    
}
