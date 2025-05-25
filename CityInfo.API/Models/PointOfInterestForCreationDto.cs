using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class PointOfInterestForCreationDto
    {
        [Required(ErrorMessage = "You should fill the name of the point of interest.")]
        [MaxLength(50, ErrorMessage = "The name of the point of interest should be less than 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "The description of the point of interest should be less than 200 characters.")]
        [Required(ErrorMessage = "You should fill the description of the point of interest.")]
        public string? Description { get; set; }
    }
}