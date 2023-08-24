using RepositoryContracts;
using ServiceContracts.BurgerDto;
using ServiceContracts.Interfaces;
namespace Services
{
    public class BurgerUpdaterService :IBurgerUpdaterService
    {
        private readonly IBurgerRepository _repository;
        public BurgerUpdaterService(IBurgerRepository BurgerRepository)
        {
            _repository = BurgerRepository;
        }

        public async Task<BurgerResponseDto> UpdateBurger(BurgerUpdateRequestDto burgerUpdateRequestDto)
        {
            
            burgerUpdateRequestDto.FixIngredientString(burgerUpdateRequestDto);
            var burger = burgerUpdateRequestDto.ToBurger();

            var updatedBurger = await _repository.UpdateBurger(burger);
            var updatedBurgerResponse = updatedBurger.ToBurgerResponseDto();
            return updatedBurgerResponse;


        }

        
    }
}
