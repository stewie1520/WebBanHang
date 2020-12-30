using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBanHang.DTOs.BasketItems;
using WebBanHang.Models;

namespace WebBanHang.DTOs.Baskets
{
    public class GetBasketDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        public BasketStatus Status { get; set; } = BasketStatus.Ordering;
        [Required]
        public int? CustomerId { get; set; }
        [Required]
        public IEnumerable<GetBasketItemDto> BasketItems { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        public int TotalPrice { get; set; }
    }
}