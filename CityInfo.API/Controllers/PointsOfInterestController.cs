using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
  [ApiController]
  [Route("api/cities/{cityId}/pointsofinterest")] // It is a child resource of an other resource (city) so we need to add the cityId to the route
  public class PointsOfInterestController : ControllerBase // It is a child resource of an other resource (city)
  {

    private readonly ILogger<PointsOfInterestController> _logger;

    public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
      throw new Exception("Test exception");
      try
      {
        //throw new Exception("Test exception");

        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
          _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest");

          return NotFound();
        }
        return Ok(city.PointsOfInterest);
      }
      catch (Exception ex)
      {
        _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
        return StatusCode(500, "A problem happened while handling your request.");
      }
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")] // Name is used to generate the URL for the resource
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city == null)
      {
        // return NotFound() will return a 404 Not Found response

        return NotFound();
      }
      var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
      if (pointOfInterest == null)
      {

        return NotFound();
      }
      return Ok(pointOfInterest);
    }

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
    {

      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city == null)
      {
        return NotFound();
      }

      var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
      var finalPointOfInterest = new PointOfInterestDto()
      {
        Id = ++maxPointOfInterestId,
        Name = pointOfInterest.Name,
        Description = pointOfInterest.Description
      };

      city.PointsOfInterest.Add(finalPointOfInterest);

      return CreatedAtRoute(nameof(GetPointOfInterest), new { cityId = cityId, pointOfInterestId = finalPointOfInterest.Id }, finalPointOfInterest);
    }

    [HttpPut("{pointOfInterestId}")]
    public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
    {
      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city == null)
      {
        return NotFound();
      }

      var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
      if (pointOfInterestFromStore == null)
      {
        return NotFound();
      }

      pointOfInterestFromStore.Name = pointOfInterest.Name;
      pointOfInterestFromStore.Description = pointOfInterest.Description;

      return NoContent(); // 204 No Content
    }


  }
}