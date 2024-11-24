using back.Data;
using back.Models.Entities;
using back.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<Product?> GetProductAsync(int id)
    {
        Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        return product;
    }
    public async Task AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteProductAsync(Product product)
    {
        _context.Remove(product);
        await _context.SaveChangesAsync();
    }

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