namespace WebApp.Model
{
    using Microsoft.EntityFrameworkCore;

    public class DataBaseContext : DbContext
    {
        public DbSet<Estate> Estates { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<OwnerType> OwnerTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./main.db");
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}