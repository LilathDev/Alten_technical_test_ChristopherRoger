using System;
using System.Threading.Tasks;
using back.Dtos.Product;
using back.Models.Entities;
using back.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Get all products from the database.
        /// </summary>
        /// <returns>A list of all products.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Get a single product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The requested product if found, or a 404 response if not.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            Product? product = await _productRepository.GetProductAsync(id);
            if (product == null)
                return NotFound(new { Message = "The product you tried to retrieve doesn't exist." });

            return Ok(product);
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="product">The product to create.</param>
        /// <returns>A success message upon creation.</returns>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest(new { Message = "Invalid product data." });
            }
            Product product = new Product
            {
                Code = productDto.Code,
                Name = productDto.Name,
                Description = productDto.Description,
                Image = productDto.Image,
                Category = productDto.Category,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                InternalReference = productDto.InternalReference,
                ShellId = productDto.ShellId,
                InventoryStatus = productDto.InventoryStatus,
                Rating = productDto.Rating,
                CreatedAt = productDto.CreatedAt,
            };
            await _productRepository.AddProductAsync(product);
            return Ok(new { Message = "Product created successfully." });
        }

        /// <summary>
        /// Delete a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>A success message if deletion is successful or an error message if not.</returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product? product = await _productRepository.GetProductAsync(id);

            if (product == null)
                return NotFound(new { Message = "No product found with the provided ID." });

            await _productRepository.DeleteProductAsync(product);
            return Ok(new { Message = "Product successfully deleted." });
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PatchProduct([FromRoute] int id, [FromBody] UpdateProductDto updatedProduct)
        {
            Product? existingProduct = await _productRepository.GetProductAsync(id);

            if (existingProduct == null)
            {
                return NotFound($"The product with ID {id} does not exist.");
            }

            // Copier uniquement les propriétés non nulles de updatedProduct vers existingProduct
            foreach (var property in typeof(UpdateProductDto).GetProperties())
            {
                var newValue = property.GetValue(updatedProduct);
                if (newValue != null) // Appliquer uniquement les propriétés spécifiées
                {
                    var targetProperty = typeof(Product).GetProperty(property.Name);
                    if (targetProperty != null && targetProperty.CanWrite)
                    {
                        targetProperty.SetValue(existingProduct, newValue);
                    }
                }
            }

            existingProduct.UpdatedAt = DateTime.UtcNow;

            // Enregistrer les modifications
            bool isUpdated = await _productRepository.UpdateProductAsync(existingProduct);

            if (!isUpdated)
            {
                return StatusCode(500, "An error occurred while updating the product.");
            }

            return NoContent();
        }
    }
}