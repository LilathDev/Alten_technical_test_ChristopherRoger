using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back.Models.Entities;
namespace back.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(Product product);

        Task<bool> UpdateProductAsync(Product product);
    }
}