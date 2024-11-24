using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace back.Models.Entities
{
    /// <summary>
    /// Represents a product in the catalog.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The unique identifier for the product.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The unique code for the product.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The description of the product.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The image of the product.
        /// </summary>
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// The category of the product.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// The price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The available quantity of the product.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The internal reference of the product.
        /// </summary>
        public string InternalReference { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the associated shell.
        /// </summary>
        public int ShellId { get; set; }

        /// <summary>
        /// The inventory status of the product.
        /// </summary>
        public InventoryStatus InventoryStatus { get; set; }

        /// <summary>
        /// The rating of the product.
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// The creation date of the product.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The last update date of the product.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Represents the inventory status of a product.
    /// </summary>
    public enum InventoryStatus
    {
        /// <summary>
        /// The product is in stock.
        /// </summary>
        [EnumMember(Value = "In Stock")]
        INSTOCK,

        /// <summary>
        /// The product is in low stock.
        /// </summary>
        [EnumMember(Value = "Low Stock")]
        LOWSTOCK,

        /// <summary>
        /// The product is out of stock.
        /// </summary>
        [EnumMember(Value = "Out of Stock")]
        OUTOFSTOCK
    }
}