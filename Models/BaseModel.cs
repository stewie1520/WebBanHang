using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public abstract class BaseModel
    {
        [Key]
        public virtual int Id { get; protected set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
