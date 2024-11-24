using back.Data;
using back.Models.Entities;
using back.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace back.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the ProductRepository class.
        /// </summary>
        /// <param name="context">The database context to be used for data operations.</param>
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously retrieves all products from the database.
        /// </summary>
        /// <returns>A list of products.</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        /// <summary>
        /// Asynchronously adds a new product to the database.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a product from the database.
        /// </summary>
        /// <param name="product">The product to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteProductAsync(Product product)
        {
            _context.Remove(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates a product in the database.
        /// </summary>
        /// <param name="product">The product with updated values.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}