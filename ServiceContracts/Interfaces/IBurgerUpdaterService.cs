using ServiceContracts.BurgerDto;

namespace ServiceContracts.Interfaces
{
    public interface IBurgerUpdaterService
    {
        /// <summary>
        /// Updates an existing Burger.
        /// </summary>
        /// <param name="BurgerUpdateRequestDto">The updated details of the Burger.</param>
        /// <returns>The updated Burger.</returns>
        Task<bool> UpdateBurger(BurgerUpdateRequestDto BurgerUpdateRequestDto);
    }
}
