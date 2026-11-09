using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public required string Name { get; set; }

        public string? Description { get; set; }
        
        [Display(Name = "Price per night")]
        [Range(10, 10000)]
        [Required]
        public double Price { get; set; }
        
        [Required]
        public int Sqft { get; set; }
        
        [Range(1, 10)]
        [Required]
        public int Occupancy { get; set; }
        
        [Display(Name = "Image Url")]
        [NotMapped]
        public  IFormFile? Image { get; set; }

        public IEnumerable<Amenity>? Amenities { get; set; }

        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
