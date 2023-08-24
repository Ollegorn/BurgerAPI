using Entities;

namespace RepositoryContracts
{
    public interface IBurgerRepository
    {
        /// <summary>
        /// Retrieves all the <see cref="Burger"/> existing in the Database.
        /// </summary>
        /// <returns>A list of <see cref="Burger"> objects.</returns>
        Task<List<Burger>> GetAllBurgers();

        /// <summary>
        /// Retrieves all the <see cref="Burger"/> with the ids provided.
        /// </summary>
        /// <param name="burgerIds">The ids of the <see cref="Burger"/> to get.</param>
        /// <returns>A list of <see cref="Burger"> objects.</returns>
        Task<List<Burger>> GetBurgersByIds(List<int> burgerIds);

        /// <summary>
        /// Retrieves a <see cref="Burger"/> based on the id provided.
        /// </summary>
        /// <param name="id">The id of the <see cref="Burger"/>.</param>
        /// <returns>A <see cref="Burger"/> object.</returns>
        Task<Burger> GetBurgerById(int id);

        /// <summary>
        /// Adds a <see cref="Burger"/> to the Database.
        /// </summary>
        /// <param name="burger">The <see cref="Burger"/> to add.</param>
        /// <returns>The added <see cref="Burger"/>.</returns>
        Task<Burger> AddBurger(Burger burger);
    }
}