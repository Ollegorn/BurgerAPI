using RepositoryContracts;
using ServiceContracts.Interfaces;

namespace Services
{
    public class BurgerDeleterService : IBurgerDeleterService
    {
        private readonly IBurgerRepository _repository;
        public BurgerDeleterService(IBurgerRepository burgerRepository)
        {
            _repository = burgerRepository;
        }
        public async Task<bool> DeleteBurgerById(int id)
        {
            //logg
            var burgerToDelete = await _repository.DeleteBurger(id);

            if (burgerToDelete)
                //logg
                return true;
            
            //logg
            return false;
        }
    }
}
