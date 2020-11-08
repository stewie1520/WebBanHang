using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class Address : BaseModel
    {
        [Required]
        public string FullName { get; set; }
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
        [Required]
        public User User { get; set; }
    }
}