using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class BurgerRepository : IBurgerRepository
    {
        private readonly BurgerDbContext _context;
        public BurgerRepository(BurgerDbContext burgerDbContext)
        {
            _context = burgerDbContext;
        }

        public async Task<Burger> AddBurger(Burger burger)
        {
            _context.Burgers.Add(burger);
            await _context.SaveChangesAsync();
            return burger;
        }

        public async Task<bool> DeleteBurger(int id)
        {
            var burger =await  _context.Burgers.FindAsync(id);
            if (burger == null)
                return false;

            _context.Burgers.Remove(burger);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Burger>> GetAllBurgers()
        {
            return await _context.Burgers.ToListAsync();
        }

        public async Task<Burger> GetBurgerById(int id)
        {
            var burger = await _context.Burgers.FindAsync(id);
            return burger;
        }

        public async Task<List<Burger>> GetBurgersByIds(List<int> burgerIds)
        {
            List<Burger> burgers = new List<Burger>();
            foreach (int id in burgerIds)
            {
                Burger existingBurger = await _context.Burgers.FirstOrDefaultAsync(b => b.Id == id);

                if (existingBurger != null)
                {
                    burgers.Add(existingBurger);
                }
            }
            return burgers;
        }

        public async Task<bool> UpdateBurger(Burger burger)
        {
            var existing = await _context.Burgers.FindAsync(burger.Id);

            if (existing == null)
                return false;

            existing.Id = burger.Id; 
            existing.Name = burger.Name;
            existing.Price = burger.Price;
            existing.Ingredients = burger.Ingredients;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}