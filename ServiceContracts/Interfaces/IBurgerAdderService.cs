using ServiceContracts.BurgerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Interfaces
{
    public interface IBurgerAdderService
    {
        /// <summary>
        /// Adds a new Burger.
        /// </summary>
        /// <param name="BurgerAddRequestDto">The details for the Burger.</param>
        /// <returns>The added Burger</returns>
        Task<BurgerResponseDto> AddBurger(BurgerAddRequestDto BurgerAddRequestDto);
    }
}
