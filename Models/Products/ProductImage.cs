using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class ProductImage : BaseModel, ISoftDelete
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public virtual Product Product { get; set; }
        [Required]
        [Url]
        public string Url { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
