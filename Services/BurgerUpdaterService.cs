using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.BurgerDto;
using ServiceContracts.Interfaces;


namespace Services
{
    public class BurgerUpdaterService :IBurgerUpdaterService
    {
        private readonly IBurgerRepository _repository;
        private readonly ILogger<BurgerGetterService> _logger;
        public BurgerUpdaterService(IBurgerRepository BurgerRepository,ILogger<BurgerGetterService> logger)
        {
            _repository = BurgerRepository;
            _logger = logger;
        }

        public async Task<bool> UpdateBurger(BurgerUpdateRequestDto burgerUpdateRequestDto)
        {
            _logger.LogInformation("Updating burger");
            if (burgerUpdateRequestDto == null )
            {
                _logger.LogInformation("Burger was null");
                throw new ArgumentNullException(nameof(burgerUpdateRequestDto));
            }

            burgerUpdateRequestDto.FixIngredientString(burgerUpdateRequestDto);
            _logger.LogInformation("Fixed ingredient string");

            var burger = burgerUpdateRequestDto.ToBurger();

            var updatedBurger =await _repository.UpdateBurger(burger);

            if (!updatedBurger)
            {
                _logger.LogInformation("Burger doesnt exist");

                return false;
            }
            _logger.LogInformation("Burger updated successfully");

            return true;


        }

        
    }
}
