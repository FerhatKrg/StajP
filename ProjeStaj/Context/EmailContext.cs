using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjeStaj.Entities;


namespace ProjeStaj.Context
{
    public class EmailContext:IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-1VER53E\\SQLEXPRESS; initial Catalog=NotikaEmailDb;integrated security=true;trust server certificate=true");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages{ get; set; }
        public DbSet<Comment> Comments{ get; set; }
    }
}
