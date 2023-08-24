using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Entities;

namespace ServiceContracts.BurgerDto
{
    public class BurgerAddRequestDto
    {

        [Required(ErrorMessage = "The BurgerName field is required.")]
        [StringLength(20, ErrorMessage = "The BurgerName field must not exceed 20 characters.")]
        public string? BurgerName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The Price field must be a positive number.")]
        [Column(TypeName = "decimal")]
        public int BurgerPrice { get; set; }

        [Required(ErrorMessage = "At least 1 ingredient is required.")]
        public string? BurgerIngredients { get; set; }


        /// <summary>
        /// Converts the <see cref="BurgerAddRequestDto"/> to a <see cref="Burger"/> object.
        /// </summary>
        /// <returns>A <see cref="Burger"/> object.</returns>
        public Burger ToBurger()
        {
            return new Burger
            {
                Name = BurgerName,
                Price = BurgerPrice,
                Ingredients = BurgerIngredients
            };
        }

        /// <summary>
        /// Converts the string the user inputs for new <see cref="Burger"/> objects into a comma seperated string.
        /// </summary>
        /// <param name="BurgerAddRequestDto"> A <see cref="BurgerAddRequestDto"/> object </param>
        /// <returns> A <see cref="BurgerAddRequestDto"/> object </returns>
        public BurgerAddRequestDto FixIngredientString(BurgerAddRequestDto BurgerAddRequestDto)
        {
            BurgerAddRequestDto.BurgerName = BurgerName;
            BurgerAddRequestDto.BurgerPrice = BurgerPrice;


            string[] ingredients = BurgerAddRequestDto.BurgerIngredients.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ingredients.Length; i++)
            {
                ingredients[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ingredients[i]);
            }
            string fixedStringIngredients = string.Join(",", ingredients);

            BurgerAddRequestDto.BurgerIngredients = fixedStringIngredients;

            return BurgerAddRequestDto;
        }


    }
}
