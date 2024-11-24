using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back.Models.Entities;

namespace back.Dtos.Product
{
    public class ProductDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        /// L'image du produit.
        /// </summary>
        public string Image { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string InternalReference { get; set; } = string.Empty;
        public int ShellId { get; set; }
        public InventoryStatus InventoryStatus { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateProductDto
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Category { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string? InternalReference { get; set; }
        public int? ShellId { get; set; }
        public Models.Entities.InventoryStatus? InventoryStatus { get; set; }
        public double? Rating { get; set; }
    }
}