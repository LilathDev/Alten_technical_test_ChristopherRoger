using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back.Models.Entities;
using back.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back.Extensions;
using back.Dtos.Cart;
namespace back.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        public CartController(ICartRepository cartRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            Guid? userId = HttpContext.GetUserId();
            if (userId == null)
                return NotFound();

            // Récupérer le panier de l'utilisateur
            Cart? cart = await _cartRepository.GetCartByUserIdAsync(userId.Value);

            // Si le panier n'existe pas, en créer un nouveau
            if (cart == null)
            {
                cart = new Cart { UserId = userId.Value };
                await _cartRepository.AddCartAsync(cart);
            }

            // Mapper le panier en DTO
            var cartDto = new CartDto
            {
                Id = cart.Id,
                Items = cart.Items.Select(item => new CartItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

            // Retourner le DTO
            return Ok(cartDto);
        }

        [HttpPost("{cartId}/items")]
        public async Task<IActionResult> AddCartItem(int cartId, [FromBody] CartItemDto cartItemDto)
        {
            // Vérifier si le panier existe
            Cart? cart = await _cartRepository.GetCartByIdAsync(cartId);
            if (cart == null)
                return NotFound("Cart not found");

            // Vérifier si le produit existe
            Product? product = await _productRepository.GetProductByIdAsync(cartItemDto.ProductId);
            if (product == null)
                return NotFound("Product not found");

            // Vérifier si un CartItem pour ce produit existe déjà
            CartItem? existingItem = cart.Items.FirstOrDefault(ci => ci.ProductId == cartItemDto.ProductId);
            if (existingItem != null)
            {
                // Si le produit est déjà dans le panier, augmenter la quantité
                existingItem.Quantity += cartItemDto.Quantity;
            }
            else
            {
                // Créer un nouvel item et l'ajouter au panier
                var newItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = cartItemDto.ProductId,
                    Quantity = cartItemDto.Quantity
                };
                cart.Items.Add(newItem);
            }

            // Sauvegarder les modifications dans le dépôt
            await _cartRepository.SaveChangesAsync();

            // Mapper l'objet Cart à un CartDto pour la réponse
            var cartDto = new CartDto
            {
                Id = cart.Id,
                Items = cart.Items.Select(item => new CartItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

            return Ok(cartDto); // Retourner le CartDto
        }
    }
}