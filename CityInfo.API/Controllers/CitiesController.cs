using System.Text.Json;
using AutoMapper;
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
    private readonly IMapper _mapper;
    private const int MaxPageSize = 20;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
      _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]

    public async Task<ActionResult<IEnumerable<CityWithoutPointofInterestsDto>>> GetCities(
    [FromQuery] string? name, [FromQuery] string? searchQuery, int pageNumber = 1, int pageSize = 5)  // It depends if you add ?name= or ?searchQuery=
    {
      if (pageSize > MaxPageSize)
      {
        pageSize = MaxPageSize;
      }


      var (cities, paginationMetaData) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
      // var results  = new List<CityWithoutPointofInterestsDto>();
      // foreach (var city in cities)
      // {
      // Here we are mapping the city to a CityWithoutPointofInterestsDto 
      //so it is error prone and we can use AutoMapper to do this automatically 
      //   results.Add(new CityWithoutPointofInterestsDto()
      //   {
      //     Id = city.Id,
      //     Name = city.Name,
      //     Description = city.Description
      //   });
      // }
      Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
      return Ok(_mapper.Map<IEnumerable<CityWithoutPointofInterestsDto>>(cities));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
    {
      var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
      if (city == null)
      {
        return NotFound();
      }
      if (includePointsOfInterest)
      {
        var result = _mapper.Map<CityDto>(city);
        //result.PointsOfInterest = _mapper.Map<IEnumerable<PointOfInterestDto>>(city.PointsOfInterest);
        return Ok(result);
        //return Ok(_mapper.Map<CityDto>(city));
      }
      return Ok(_mapper.Map<CityWithoutPointofInterestsDto>(city));
    }
  }
}