﻿using Entities;
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
    }
}