using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class ProductOrder : BaseModel
    {
        [Required]
        public Order Order { get; set; }
        [Required]
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}