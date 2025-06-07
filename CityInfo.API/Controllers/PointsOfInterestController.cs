using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
  [ApiController]
  [Route("api/cities/{cityId}/pointsofinterest")] // It is a child resource of an other resource (city) so we need to add the cityId to the route
  public class PointsOfInterestController : ControllerBase // It is a child resource of an other resource (city)
  {

    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly IMailService _mailService;
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
    IMailService mailService,
    ICityInfoRepository cityInfoRepository,
    IMapper mapper)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
      _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
    {
      if (!await _cityInfoRepository.CityExistsAsync(cityId))
      {

        _logger.LogInformation($"City with id {cityId} not found when accessing points of interest.");
        // return NotFound() will return a 404 Not Found response
        return NotFound();
      }
      var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);
      if (pointsOfInterestForCity == null || !pointsOfInterestForCity.Any())
      {
        return NotFound();
      }
      return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")] // Name is used to generate the URL for the resource
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId) // We need to add the cityId to the route to get the city
    {
      var city = await _cityInfoRepository.GetCityAsync(cityId, true);
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
      return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
    }

    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
    {

      if (!await _cityInfoRepository.CityExistsAsync(cityId))
      {
        _logger.LogInformation($"City with id {cityId} not found when accessing points of interest.");
        // return NotFound() will return a 404 Not Found response
        return NotFound();
      }

      var newPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);
      await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, newPointOfInterest);
      await _cityInfoRepository.SaveChangesAsync();

      var createdPointOfInterestToReturn = _mapper.Map<PointOfInterestDto>(newPointOfInterest);

      return CreatedAtRoute("GetPointOfInterest",
      new { cityId = cityId, pointOfInterestId = createdPointOfInterestToReturn.Id },
      createdPointOfInterestToReturn);
    }

    [HttpPut("{pointOfInterestId}")]
    public async Task<ActionResult> UpdatePointOfInterest(int cityId,
    int pointOfInterestId,
    PointOfInterestForUpdateDto pointOfInterest)
    {
      if (!await _cityInfoRepository.CityExistsAsync(cityId))
      {
        _logger.LogInformation($"City with id {cityId} not found when updating point of interest.");
        return NotFound();
      }

      var pointOfInterestFromStore = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
      if (pointOfInterestFromStore == null)
      {
        return NotFound();
      }

      _mapper.Map(pointOfInterest, pointOfInterestFromStore); // Update the point of interest from the store with the new point of interest

      await _cityInfoRepository.SaveChangesAsync();
      return NoContent(); // 204 No Content
    }

    [HttpPatch("{pointOfInterestId}")]
    public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
      if (!await _cityInfoRepository.CityExistsAsync(cityId))
      {
        return NotFound();
      }

      var pointOfInterestFromStore = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
      if (pointOfInterestFromStore == null)
      {
        return NotFound();
      }

      var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestFromStore);

      patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (!TryValidateModel(pointOfInterestToPatch))
      {
        return BadRequest(ModelState);
      }

      _mapper.Map(pointOfInterestToPatch, pointOfInterestFromStore);

      await _cityInfoRepository.SaveChangesAsync();

      return NoContent();
    }

    [HttpDelete("{pointOfInterestId}")]
    public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
      var city = await _cityInfoRepository.GetCityAsync(cityId, false);
      if (city == null)
      {
        return NotFound();
      }

      var pointOfInterestFromStoreTask = _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
      if (pointOfInterestFromStoreTask == null)
      {
        return NotFound();
      }

      var pointOfInterestFromStore = await pointOfInterestFromStoreTask;

      // Remove the point of interest from the city
      _cityInfoRepository.DeletePointOfInterest(pointOfInterestFromStore);
      await _cityInfoRepository.SaveChangesAsync();

      _mailService.Send("Point of interest deleted.", $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");

      return NoContent(); // 204 No Content
    }
  }
}