using RepositoryContracts;
using ServiceContracts.BurgerDto;
using ServiceContracts.Interfaces;

namespace Services
{
    public class BurgerGetterService : IBurgerGetterService
    {
        private readonly IBurgerRepository _repository;
        public BurgerGetterService(IBurgerRepository burgerRepository)
        {
            _repository = burgerRepository;
        }

        public async Task<List<BurgerResponseDto>> GetAllBurgers()
        {
            //logg
            var burgers = await _repository.GetAllBurgers();
            var burgersResponse = burgers.ToBurgerResponseDtoList();
            //logg
            return burgersResponse;
        }

        public async Task<BurgerResponseDto> GetBurgerById(int id)
        {
            //logg
            var burger = await _repository.GetBurgerById(id);
            var burgerResponse = burger.ToBurgerResponseDto();
            //logg
            return burgerResponse;
        }

        public async Task<List<BurgerResponseDto>> GetBurgersByIds(List<int> ids)
        {
            //logg
            var burgers = await _repository.GetBurgersByIds(ids);
            var burgersResponse = burgers.ToBurgerResponseDtoList();
            //logg
            return burgersResponse;
        }
    }
}