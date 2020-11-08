using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebBanHang.Models
{
    public class Order : BaseModel
    {
        [Required]
        public int Amount { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string District { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Ordering;
        public string Ward { get; set; }
        public User User { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<ProductOrder> ProductOrders { get; set; }
    }
}