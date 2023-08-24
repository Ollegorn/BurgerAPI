using RepositoryContracts;
using ServiceContracts.BurgerDto;
using ServiceContracts.Interfaces;


namespace Services
{
    public class BurgerAdderService : IBurgerAdderService
    {
        private readonly IBurgerRepository _BurgerRepository;
        public BurgerAdderService(IBurgerRepository BurgerRepository)
        {
            _BurgerRepository = BurgerRepository;
        }

        public async Task<BurgerResponseDto> AddBurger(BurgerAddRequestDto BurgerAddRequestDto)
        {
            BurgerAddRequestDto.FixIngredientString(BurgerAddRequestDto);
            var Burger = BurgerAddRequestDto.ToBurger();

            //Burger.BurgerIngredients = JsonSerializer.Serialize<string>(BurgerAddRequestDto.BurgerIngredients); NOT NEEDED
            var addedBurger = await _BurgerRepository.AddBurger(Burger);
            var addedBurgerResponse = addedBurger.ToBurgerResponseDto();
            return addedBurgerResponse;

        }
    }
}
