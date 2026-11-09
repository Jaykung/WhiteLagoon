using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.ViewModels
{
    public class AmenityVM
    {
        public required Amenity Amenity { get; set; }
        public required IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
