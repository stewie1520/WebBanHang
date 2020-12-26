using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;

using System;

namespace WebBanHang.DTOs.Customers
{
    public class CreateCustomerDto
    {
        [Required]
        public string FullName { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required]
        public CreateAddressDto Address { get; set;}
        [Required]
        public bool IsRegisted { get; set; }
        [Required]
        public Gender Gender { get; set; } = Gender.Unknown;
    }
}