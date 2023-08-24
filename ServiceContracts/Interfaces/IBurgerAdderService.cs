using ServiceContracts.BurgerDto;

namespace ServiceContracts.Interfaces
{
    public interface IBurgerAdderService
    {
        /// <summary>
        /// Adds a new Burger.
        /// </summary>
        /// <param name="BurgerAddRequestDto">The details for the Burger.</param>
        /// <returns>The added Burger</returns>
        Task<BurgerResponseDto> AddBurger(BurgerAddRequestDto BurgerAddRequestDto);
    }
}
