using Entities;

namespace ServiceContracts.BurgerDto
{
    public class BurgerResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public string? Ingredients { get; set; }


        /// <summary>
        /// Converts the <see cref="BurgerResponseDto"/> object to a <see cref="Burger"/> object.
        /// </summary>
        /// <returns>A <see cref="Burger"/> object.</returns>
        public Burger ToBurger()
        {
            return new Burger
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Ingredients = Ingredients
            };
        }


        /// <summary>
        /// Compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">The BurgerResponseDto Object to compare</param>
        /// <returns>True or false, indicating whether all burger details are matched with the specified parameter object</returns>
        public override bool Equals(object obj)
        {
            if (obj is null || GetType() != obj.GetType())
                return false;

            BurgerResponseDto other = (BurgerResponseDto)obj;
            return Id == other.Id &&
                   Ingredients == other.Ingredients &&
                   Name == other.Name &&
                   Price == other.Price;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


    }

    /// <summary>
    /// Extention method for the <see cref="Burger"/> object.
    /// </summary>
    public static class OrderExtentions
    {
        /// <summary>
        /// Converts the <see cref="Burger"/> object to a <see cref="BurgerResponseDto"/> object.
        /// </summary>
        /// <param name="Burger">The <see cref="Burger"/> object to convert.</param>
        /// <returns>A <see cref="BurgerResponseDto"/> object.</returns>
        public static BurgerResponseDto ToBurgerResponseDto(this Burger Burger)
        {
            return new BurgerResponseDto
            {
                Id = Burger.Id,
                Name = Burger.Name,
                Price = Burger.Price,
                Ingredients = Burger.Ingredients
            };
        }

        /// <summary>
        /// Converts a list of <see cref="Burger"/> objects to a list of <see cref="BurgerResponseDto"/> objects.
        /// </summary>
        /// <param name="Burgers">The list of <see cref="Burger"/> objects to convert.</param>
        /// <returns>A list of <see cref="BurgerResponseDto"/> objects.</returns>
        public static List<BurgerResponseDto> ToBurgerResponseDtoList(this List<Burger> Burgers)
        {
            var BurgersResponseDto = new List<BurgerResponseDto>();
            {
                foreach (var Burger in Burgers)
                {
                    BurgersResponseDto.Add(Burger.ToBurgerResponseDto());
                }
                return BurgersResponseDto;
            }
        }
    }
}
