using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;

namespace WebBanHang.DTOs.Products
{
    public class UpdateProductDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public bool IsVariant { get; set; } = false;
        public bool IsManageVariant { get; set; } = false;
        [Required]
        public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public List<string> ImageUrls { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Private;
    }
}
