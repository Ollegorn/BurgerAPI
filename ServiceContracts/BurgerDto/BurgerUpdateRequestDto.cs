using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Globalization;

namespace ServiceContracts.BurgerDto
{
    public class BurgerUpdateRequestDto
    {
        [Required]
        public int BurgerId { get; set; }

        [Required(ErrorMessage = "The BurgerName field is required.")]
        [StringLength(20, ErrorMessage = "The BurgerName field must not exceed 20 characters.")]
        public string? BurgerName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The Price field must be a positive number.")]
        [Column(TypeName = "decimal")]
        public int BurgerPrice { get; set; }

        [Required(ErrorMessage = "At least 1 ingredient is required.")]
        public string? BurgerIngredients { get; set; }


        /// <summary>
        /// Converts the <see cref="BurgerUpdateRequestDto"/> to a <see cref="Burger"/> object.
        /// </summary>
        /// <returns>A <see cref="Burger"/> object.</returns>
        public Burger ToBurger()
        {
            return new Burger
            {
                Id = BurgerId,
                Name = BurgerName,
                Price = BurgerPrice,
                Ingredients = BurgerIngredients
            };
        }

        /// <summary>
        /// Converts the string the user inputs for new <see cref="Burger"/> objects into a comma seperated string.
        /// </summary>
        /// <param name="BurgerUpdateRequestDto"> A <see cref="BurgerUpdateRequestDto"/> object. </param>
        /// <returns> A <see cref="BurgerUpdateRequestDto"/> object. </returns>
        public BurgerUpdateRequestDto FixIngredientString(BurgerUpdateRequestDto burgerUpdateRequestDto)
        {
            burgerUpdateRequestDto.BurgerName = BurgerName;
            burgerUpdateRequestDto.BurgerPrice = BurgerPrice;


            string[] ingredients = burgerUpdateRequestDto.BurgerIngredients.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ingredients.Length; i++)
            {
                ingredients[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ingredients[i]);
            }
            string fixedStringIngredients = string.Join(",", ingredients);

            burgerUpdateRequestDto.BurgerIngredients = fixedStringIngredients;

            return burgerUpdateRequestDto;
        }
    }
}
