using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class ProductImage : BaseModel
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public virtual Product Product { get; set; }
        [Required]
        [Url]
        public string Url { get; set; }
    }
}
