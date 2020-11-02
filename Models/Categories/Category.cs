using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebBanHang.Models
{
    public class Category : BaseModel
    {
        [Range(1, 3)]
        [Required]
        public int Tier { get; set; } = 3;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public Category Parent { get; set; }

        public IEnumerable<Category> Children { get; set; }
    }
}