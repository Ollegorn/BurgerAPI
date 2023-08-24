using ServiceContracts.BurgerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Interfaces
{
    public interface IBurgerUpdaterService
    {
        /// <summary>
        /// Updates an existing Burger.
        /// </summary>
        /// <param name="BurgerUpdateRequestDto">The updated details of the Burger.</param>
        /// <returns>The updated Burger.</returns>
        Task<BurgerResponseDto> UpdateBurger(BurgerUpdateRequestDto BurgerUpdateRequestDto);
    }
}
