

namespace ServiceContracts.Interfaces
{
    public interface IBurgerDeleterService
    {
        /// <summary>
        /// Deletes the Burger with the given id.
        /// </summary>
        /// <param name="id">The id of the Burger to be deleted.</param>
        /// <returns>True or false.</returns>
        Task<bool> DeleteBurgerById(int id);
    }
}
