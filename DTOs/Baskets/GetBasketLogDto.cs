using System;
using WebBanHang.Models;

namespace WebBanHang.DTOs.Baskets
{
  public class GetBasketLogDto
  {
    public int BasketId { get; set; }
    public BasketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}
