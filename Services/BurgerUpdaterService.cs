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

        public async Task<bool> UpdateBurger(BurgerUpdateRequestDto burgerUpdateRequestDto)
        {
            //logg
            burgerUpdateRequestDto.FixIngredientString(burgerUpdateRequestDto);
            //logg
            var burger = burgerUpdateRequestDto.ToBurger();

            var updatedBurger =await _repository.UpdateBurger(burger);

            if(updatedBurger)
            //logg
                return true;

            return false;


        }

        
    }
}
