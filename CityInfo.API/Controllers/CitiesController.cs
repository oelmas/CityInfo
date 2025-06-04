using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
  [ApiController]

  [Route("api/cities")]
  public class CitiesController : ControllerBase
  {
    private readonly ICityInfoRepository _cityInfoRepository;

    public CitiesController(ICityInfoRepository cityInfoRepository)
    {
      _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
    }

    [HttpGet]

    public async Task<ActionResult<IEnumerable<CityWithoutPointofInterestsDto>>> GetCities()
    {
      var cities = await _cityInfoRepository.GetCitiesAsync();

      var results  = new List<CityWithoutPointofInterestsDto>();
      foreach (var city in cities)
      {
        // Here we are mapping the city to a CityWithoutPointofInterestsDto 
        //so it is error prone and we can use AutoMapper to do this automatically 
        results.Add(new CityWithoutPointofInterestsDto()
        {
          Id = city.Id,
          Name = city.Name,
          Description = city.Description
        });
      }
      return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CityDto>> GetCity(int id)
    {
      // var cityToReturn = await _cityInfoRepository.GetCityAsync(id, false);
      // if (cityToReturn == null)
      // {
      //   return NotFound();
      // }
      // return Ok(cityToReturn);
      return Ok();
    }
  }
}