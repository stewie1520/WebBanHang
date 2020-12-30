using System;

using WebBanHang.Models;

namespace WebBanHang.DTOs.Baskets
{
  public class QueryBasketDto
  {
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }

    public BasketStatus? Status { get; set; }
  }
}
