using System;
using System.ComponentModel.DataAnnotations;

namespace FoodieDelight_Api.Data
{
	public class Restaurant
	{
		
        [Key]
        public int RestroId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Location { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string CuisineType { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Contact number is not valid")]
        public string ContactNumber { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string OperatingHours { get; set; }

        
        [Url]
        public string Website { get; set; }


        [RegularExpression(@"^\d+$", ErrorMessage = "Price range must be a positive number")]
        public string PriceRange { get; set; }

        [Range(0.0, 5.0)]
        public double? AverageRating { get; set; }

        public List<MenuItem> MenuItems { get; set; }
        
    }

    public class MenuItem
    {
        [Key]
        public int MenuId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public double Price { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } // Navigation property

    }

}


