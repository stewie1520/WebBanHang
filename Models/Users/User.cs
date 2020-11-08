using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class User : BaseAccount
    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public IEnumerable<Address> Addresses { get; set; }
    }
}
