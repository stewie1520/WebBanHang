using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;

namespace WebBanHang.DTOs.Baskets
{
    public class UpdateBasketStatusDto
    {
        [Required]   
        public int Id { get; set; }
        [Required]
        public BasketStatus Status { get; set; } = BasketStatus.Delivered;
    }
}