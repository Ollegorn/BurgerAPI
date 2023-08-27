using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.BurgerDto;
using ServiceContracts.Interfaces;


namespace Services
{
    public class BurgerAdderService : IBurgerAdderService
    {
        private readonly IBurgerRepository _BurgerRepository;
        private readonly ILogger<BurgerAdderService> _logger;
        public BurgerAdderService(IBurgerRepository BurgerRepository,ILogger<BurgerAdderService> logger)
        {
            _BurgerRepository = BurgerRepository;
            _logger = logger;
        }

        public async Task<BurgerResponseDto> AddBurger(BurgerAddRequestDto BurgerAddRequestDto)
        {
            _logger.LogInformation("Adding a new burger");

            BurgerAddRequestDto.FixIngredientString(BurgerAddRequestDto);
            var Burger = BurgerAddRequestDto.ToBurger();
            
            _logger.LogInformation("Fixxed ingredient string");

            var addedBurger = await _BurgerRepository.AddBurger(Burger);
            var addedBurgerResponse = addedBurger.ToBurgerResponseDto();

            _logger.LogInformation("Burger added successfully");

            return addedBurgerResponse;

        }
    }
}
