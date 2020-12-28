using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.BasketItems
{
    public class GetBasketItemDto
    {
        [Required]
        public int BasketId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}