using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models.Products
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
