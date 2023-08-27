using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.BurgerDto;
using ServiceContracts.Interfaces;

namespace Services
{
    public class BurgerGetterService : IBurgerGetterService
    {
        private readonly IBurgerRepository _repository;
        private readonly ILogger<BurgerGetterService> _logger;
        public BurgerGetterService(IBurgerRepository burgerRepository,ILogger<BurgerGetterService> logger)
        {
            _repository = burgerRepository;
            _logger = logger;
        }

        public async Task<List<BurgerResponseDto>> GetAllBurgers()
        {
            _logger.LogInformation("Getting all burgers");

            var burgers = await _repository.GetAllBurgers();
            var burgersResponse = burgers.ToBurgerResponseDtoList();

            _logger.LogInformation("Burgers retrieved successfully");

            return burgersResponse;
        }

        public async Task<BurgerResponseDto> GetBurgerById(int id)
        {
            _logger.LogInformation("Getting burger with given id");

            var burger = await _repository.GetBurgerById(id);
            var burgerResponse = burger?.ToBurgerResponseDto();

            _logger.LogInformation("Retrieved successfully");

            return burgerResponse;
        }

        public async Task<List<BurgerResponseDto>> GetBurgersByIds(List<int> ids)
        {
            _logger.LogInformation("Getting all burgers with given ids");

            var burgers = await _repository.GetBurgersByIds(ids);
            var burgersResponse = burgers.ToBurgerResponseDtoList();

            _logger.LogInformation("Retrieved all burgers with given ids successfully");

            return burgersResponse;
        }
    }
}