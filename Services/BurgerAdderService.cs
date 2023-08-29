using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.BurgerDto;
using ServiceContracts.Interfaces;


namespace Services
{
    public class BurgerAdderService : IBurgerAdderService
    {
        private readonly IBurgerRepository _BurgerRepository;
        private readonly ILogger<BurgerGetterService> _logger;
        public BurgerAdderService(IBurgerRepository BurgerRepository,ILogger<BurgerGetterService> logger)
        {
            _BurgerRepository = BurgerRepository;
            _logger = logger;
        }

        public async Task<BurgerResponseDto> AddBurger(BurgerAddRequestDto burgerAddRequestDto)
        {
            _logger.LogInformation("Adding a new burger");
            try
            {

                if (burgerAddRequestDto == null)
                {
                    throw new ArgumentNullException(nameof(burgerAddRequestDto));
                }

                burgerAddRequestDto.FixIngredientString(burgerAddRequestDto);
                var Burger = burgerAddRequestDto.ToBurger();

                _logger.LogInformation("Fixxed ingredient string");

                var addedBurger = await _BurgerRepository.AddBurger(Burger);
                var addedBurgerResponse = addedBurger.ToBurgerResponseDto();

                _logger.LogInformation("Burger added successfully");

                return addedBurgerResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding burger, {ex.Message}");
                throw;
            }

        }
    }
}
