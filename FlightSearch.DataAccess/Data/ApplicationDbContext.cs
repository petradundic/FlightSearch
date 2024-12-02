using FlightSearch.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightSearch.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<SearchParameter> SearchParameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.SearchParameter)        
                .WithMany(sp => sp.FlightResults)      
                .HasForeignKey(f => f.SearchParameterId); 
        }
    }
}
