using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.BasketItems
{
    public class CreateBasketItemDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}