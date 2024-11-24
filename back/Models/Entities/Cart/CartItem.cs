using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models.Entities
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the associated cart.
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Navigation property to the cart.
        /// </summary>
        public Cart Cart { get; set; } = null!;

        /// <summary>
        /// Foreign key to the associated product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Navigation property to the product.
        /// </summary>
        public Product Product { get; set; } = null!;

        /// <summary>
        /// The quantity of the product in the cart.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The date when the item was added to the cart.
        /// </summary>
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}