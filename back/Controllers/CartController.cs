using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back.Models.Entities;
using back.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back.Extensions;
namespace back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        public CartController(ICartRepository cartRepository, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            Guid? userId = HttpContext.GetUserId();
            if (userId == null)
                return NotFound();
            Cart? cart = await _cartRepository.GetCartByUserIdAsync(userId.Value);

            if (cart == null)
            {
                cart = new Cart { UserId = userId.Value };
                await _cartRepository.AddCartAsync(cart);
            }

            return Ok(cart);
        }
    }
}