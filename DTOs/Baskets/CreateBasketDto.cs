using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBanHang.DTOs.BasketItems;
using WebBanHang.DTOs.Customers;
using WebBanHang.Models;

namespace WebBanHang.DTOs.Baskets 
{
    public class CreateBasketDto {
        [Required]
        public bool IsPaid { get; set; }
        public BasketStatus Status { get; set; } = BasketStatus.Ordering;
        [Required]
        public List<CreateBasketItemDto> Items { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public CreateAddressDto Address { get; set; }
        [Required]
        public string Note { get; set; }
    }
}