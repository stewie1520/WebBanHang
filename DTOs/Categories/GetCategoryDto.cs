using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.Categories
{
    public class GetCategoryDto
    {
        [Required]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [Range(1, 3)]
        [Required]
        public int Tier { get; set; } = 3;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
