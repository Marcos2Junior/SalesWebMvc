using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [Range(0.1, 50000.0, ErrorMessage = " {0} must be from {1} to {2}")]
        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Price { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public Product()
        { }

        public Product(int id, string name, string description, double price, Category category)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }
    }
}
