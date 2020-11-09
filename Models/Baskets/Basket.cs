using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebBanHang.Models
{
    public class Basket : BaseModel
    {
        [Required]
        public int TotalPrice { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        [Required]
        public BasketStatus Status { get; set; } = BasketStatus.Ordering;
        public string Note { get; set; }
        // TODO: Add property indicates which staff confirmed this order
        public IEnumerable<BasketItem> BasketItems { get; private set; }
        [Required]
        public Customer Customer { get; set; }
    }
}
