using ServiceContracts.BurgerDto;
using Entities;

namespace ServiceContracts.Interfaces
{
    /// <summary>
    /// Represents a service for retrieving Burgers
    /// </summary>
    public interface IBurgerGetterService
    {
        /// <summary>
        /// Asks the repository to get all the <see cref="Burger"/> from the Database.
        /// </summary>
        /// <returns>A list of <see cref="BurgerResponseDto"/>.</returns>
        Task<List<BurgerResponseDto>> GetAllBurgers();
        /// <summary>
        /// Asks the repository for the <see cref="Burger"/> with the specific id.
        /// </summary>
        /// <param name="id">The id of the <see cref="Burger"/>.</param>
        /// <returns>A <see cref="BurgerResponseDto"/> object.</returns>
        Task<BurgerResponseDto> GetBurgerById(int id);
        /// <summary>
        /// Asks the repository to get all the <see cref="Burger"/> based on the ids from the Database.
        /// </summary>
        /// <param name="ids">A list of <see cref="Burger"/> ids.</param>
        /// <returns>A list of <see cref="BurgerResponseDto"/> objects.</returns>
        Task<List<BurgerResponseDto>> GetBurgersByIds(List<int> ids);
    }
}
