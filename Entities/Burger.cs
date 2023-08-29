using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Burger
    {
        [Required]
        public int Id { get; set; }

        
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(20, ErrorMessage = "The Name field must not exceed 20 characters.")]
        public string? Name { get; set; }


        [Range(0, double.MaxValue, ErrorMessage = "The Price field must be a positive number.")]
        [Column(TypeName = "decimal")]
        [Required]
        public int Price { get; set; }


        [Required(ErrorMessage = "At least 1 ingredient is required.")]
        public string? Ingredients { get; set; }


    }
}