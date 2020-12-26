using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;
using System;

namespace WebBanHang.DTOs.Customers
{
    public class GetAddressDto
    {
        [Required]
        public int Id { get; set; }
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

        public int CustomerId { get; set; }
    }
}