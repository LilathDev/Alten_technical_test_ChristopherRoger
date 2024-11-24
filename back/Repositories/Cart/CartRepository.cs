using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back.Data;
using back.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace back.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the CartRepository class.
        /// </summary>
        /// <param name="context">The database context to be used for data operations.</param>
        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously adds a new cart to the database.
        /// </summary>
        /// <param name="cart">The cart to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a cart by its ID.
        /// </summary>
        /// <param name="carId">The ID of the cart to retrieve.</param>
        /// <returns>The cart if found; otherwise, null.</returns>
        public async Task<Cart?> GetCartByIdAsync(int carId)
        {
            return await _context.Carts.FirstOrDefaultAsync(x => x.Id == carId);
        }

        /// <summary>
        /// Retrieves a cart by the user ID, including its associated cart items.
        /// This method performs explicit eager loading to retrieve the related CartItems.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart is to be retrieved.</param>
        /// <returns>The cart associated with the user, including cart items, or null if not found.</returns>
        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                        .Include(c => c.Items) // Include cart items explicitly
                        .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        /// <summary>
        /// Asynchronously saves the changes made to the context.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}