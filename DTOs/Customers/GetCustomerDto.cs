using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;
using System;

namespace WebBanHang.DTOs.Customers
{
    public class GetCustomerDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required]
        public bool IsRegisted { get; set; }
        [Required]
        public Gender Gender { get; set; } = Gender.Unknown;
    }
}