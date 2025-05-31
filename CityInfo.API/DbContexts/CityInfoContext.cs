using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;
    }
}

git config --global user.email "oelmas@gmail.com"
  git config --global user.name "Öner Elmas"