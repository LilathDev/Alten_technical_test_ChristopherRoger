using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace back.Models.Entities
{
    public class Cart
    {
        /// <summary>
        /// Gets or sets the unique identifier for the cart.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user associated with the cart.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the cart.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of items in the cart.
        /// </summary>
        public List<CartItem> Items { get; set; } = new();

        /// <summary>
        /// Gets or sets the date and time when the cart was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the cart was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}