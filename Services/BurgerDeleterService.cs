using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.Interfaces;

namespace Services
{
    public class BurgerDeleterService : IBurgerDeleterService
    {
        private readonly IBurgerRepository _repository;
        private readonly ILogger<BurgerDeleterService> _logger;
        public BurgerDeleterService(IBurgerRepository burgerRepository, ILogger<BurgerDeleterService> logger)
        {
            _repository = burgerRepository;
            _logger = logger;
        }
        public async Task<bool> DeleteBurgerById(int id)
        {
            _logger.LogInformation("Deleting a new burger");

            var burgerToDelete = await _repository.DeleteBurger(id);

            if (burgerToDelete)
            {
                _logger.LogInformation("Burger not found");

                return true;
            }
            _logger.LogInformation("Added burger successfully");

            return false;
        }
    }
}
