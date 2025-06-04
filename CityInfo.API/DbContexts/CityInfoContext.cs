using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // Örnek veriler ekleme
            modelBuilder.Entity<City>().HasData(
                new City("New York") { Id = 1, Description = "The Big Apple" },
                new City("Los Angeles") { Id = 2, Description = "City of Angels" },
                new City("Paris") { Id = 3, Description = "The City of Love" },
                new City("London") { Id = 4, Description = "The Capital of England" },
                new City("Tokyo") { Id = 5, Description = "The City of Flowers" },
                new City("Berlin") { Id = 6, Description = "The Capital of Germany" }
            );

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Statue of Liberty") { Id = 1, CityId = 1, Description = "A colossal neoclassical sculpture on Liberty Island in New York City." },
                new PointOfInterest("Hollywood Sign") { Id = 2, CityId = 2, Description = "An American cultural icon located in Los Angeles, California." },
                new PointOfInterest("Eiffel Tower") { Id = 3, CityId = 3, Description = "An iron lattice tower on the Champ de Mars in Paris, France." },
                new PointOfInterest("Big Ben") { Id = 4, CityId = 4, Description = "The nickname for the Great Bell of the clock at the north end of the Palace of Westminster in London." },
                new PointOfInterest("Shibuya Crossing") { Id = 5, CityId = 5, Description = "A famous pedestrian scramble in Tokyo, Japan." },
                new PointOfInterest("Brandenburg Gate") { Id = 6, CityId = 6, Description = "An 18th-century neoclassical monument in Berlin, Germany." }

            );
        }



    }
}
