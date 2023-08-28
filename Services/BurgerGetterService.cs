using Entities;
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

        public virtual async Task<List<BurgerResponseDto>> GetAllBurgers()
        {
            _logger.LogInformation("Getting all burgers");
            try
            {
                var burgers = await _repository.GetAllBurgers();
                var burgersResponse = burgers.ToBurgerResponseDtoList();

                _logger.LogInformation("Burgers retrieved successfully");

                return burgersResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting burgers {ex.Message}");
                return null;
            }
        }

        public virtual async Task<BurgerResponseDto?> GetBurgerById(int? id)
        {
            _logger.LogInformation("Getting burger with given id");
            if (id == null)
            {
                _logger.LogInformation("Id was null");
                return null;
            }
            try
            {

                Burger? burger = await _repository.GetBurgerById(id.Value);
                if (burger == null)
                {
                    _logger.LogInformation("Burger doesn't exist");
                    return null;
                }

                var burgerResponse = burger.ToBurgerResponseDto();

                _logger.LogInformation("Retrieved successfully");

                return burgerResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting burger with id : {id}, {ex.Message}");
                return null;
            }
        }

        public virtual async Task<List<BurgerResponseDto>?> GetBurgersByIds(List<int>? ids)
        {
            _logger.LogInformation("Getting all burgers with given ids");
            try
            {
                if (ids == null)
                {
                    _logger.LogInformation("Ids were null");
                    return null;
                }
                var burgers = await _repository.GetBurgersByIds(ids);
                var burgersResponse = burgers.ToBurgerResponseDtoList();

                _logger.LogInformation("Retrieved all burgers with given ids successfully");

                return burgersResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting burgers with ids, {ex.Message}");
                return null;
            }
        }
    }
}