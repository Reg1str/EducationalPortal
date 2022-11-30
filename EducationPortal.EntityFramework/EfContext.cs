namespace EducationPortal.EntityFramework
{
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;

    public class EfContext : DbContext
    {
        private string connectionString;

        public EfContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
