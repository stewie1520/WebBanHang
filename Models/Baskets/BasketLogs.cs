using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebBanHang.Models
{
  public class BasketLog : BaseModel, ISoftDelete
  {
    [Required]
    public int BasketId { get; set; }
    [Required]
    public BasketStatus Status { get; set; }
    public bool IsDeleted { get; set; } = false;
  }
}
