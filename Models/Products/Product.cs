using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class Product : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public bool IsVariant { get; set; } = false;
        public bool IsManageVariant { get; set; } = false;
        [Required]
        public Category Category { get; set; }
        public Product Parent { get; set; }
        public IEnumerable<Product> Children { get; set; }
        public IEnumerable<ProductImage> Images { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Private;
    }
}
