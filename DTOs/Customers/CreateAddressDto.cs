using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;
using System;

namespace WebBanHang.DTOs.Customers
{
    public class CreateAddressDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string Ward { get; set; }

    }
}
