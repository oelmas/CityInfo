using CityInfo.API.Models;

namespace CityInfo.API
{
  public class CitiesDataStore
  {
    public List<CityDto> Cities { get; set; }

    public static CitiesDataStore Current { get; } = new CitiesDataStore();

    public CitiesDataStore()
    {
      Cities = new List<CityDto>
      {
        new CityDto
        {
          Id = 1,
          Name = "New York City",
          Description = "The one with that big park.",
          PointsOfInterest = new List<PointOfInterestDto>
          {
            new PointOfInterestDto { Id = 1, Name = "Central Park", Description = "The most famous park in the world." },
            new PointOfInterestDto { Id = 2, Name = "Statue of Liberty", Description = "Iconic National Monument." }
          }
        },
        new CityDto
        {
          Id = 2,
          Name = "Antwerp",
          Description = "The one with the cathedral that was never really finished.",
          PointsOfInterest = new List<PointOfInterestDto>
          {
            new PointOfInterestDto { Id = 3, Name = "Cathedral of Our Lady", Description = "A Gothic style cathedral." },
            new PointOfInterestDto { Id = 4, Name = "Antwerp Zoo", Description = "One of the oldest zoos in the world." }
          }
        },
        new CityDto
        {
          Id = 3,
          Name = "Paris",
          Description = "The one with that big tower.",
          PointsOfInterest = new List<PointOfInterestDto>
          {
            new PointOfInterestDto { Id = 5, Name = "Eiffel Tower", Description = "The most famous tower in the world." },
            new PointOfInterestDto { Id = 6, Name = "Louvre Museum", Description = "World's largest art museum." }
          }
        },
        new CityDto
        {
          Id = 4,
          Name = "Antalya",
          Description = "The one with the big castle.",
          PointsOfInterest = new List<PointOfInterestDto>
          {
            new PointOfInterestDto { Id = 7, Name = "Kaleiçi", Description = "Historic city center." },
            new PointOfInterestDto { Id = 8, Name = "Düden Waterfalls", Description = "Famous natural waterfalls." }
          }
        },
        new CityDto
        {
          Id = 5,
          Name = "Izmir",
          Description = "The one with the big port.",
          PointsOfInterest = new List<PointOfInterestDto>
          {
            new PointOfInterestDto { Id = 9, Name = "Konak Square", Description = "Famous city square." },
            new PointOfInterestDto { Id = 10, Name = "Kadifekale", Description = "Ancient castle on a hill." }
          }
        },
        new CityDto
        {
          Id = 6,
          Name = "Bursa",
          Description = "The one with the big bazaar.",
          PointsOfInterest = new List<PointOfInterestDto>
          {
            new PointOfInterestDto { Id = 11, Name = "Grand Mosque", Description = "Historic mosque in the city center." },
            new PointOfInterestDto { Id = 12, Name = "Koza Han", Description = "Famous silk bazaar." }
          }
        },
        new CityDto
        {
          Id = 9,
          Name = "Konya",
          Description = "The one with the big mosque.",
          PointsOfInterest = new List<PointOfInterestDto>
          {
            new PointOfInterestDto { Id = 13, Name = "Mevlana Museum", Description = "Museum and mausoleum of Rumi." },
            new PointOfInterestDto { Id = 14, Name = "Alaeddin Hill", Description = "Historical park and mosque." }
          }
        },
        new CityDto
        {
          Id = 10,
          Name = "Kayseri",
          Description = "The one with the big castle.",
          PointsOfInterest = new List<PointOfInterestDto>
          {
            new PointOfInterestDto { Id = 15, Name = "Kayseri Castle", Description = "Historic castle in the city center." },
            new PointOfInterestDto { Id = 16, Name = "Erciyes Ski Resort", Description = "Popular ski destination." }
          }
        },
      };
    }
  }
}

