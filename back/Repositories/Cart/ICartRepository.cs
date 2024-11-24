using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back.Models.Entities;

namespace back.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(Guid userId);
        Task<Cart?> GetCartByIdAsync(int carId);
        Task AddCartAsync(Cart cart);
        Task SaveChangesAsync();
    }
}