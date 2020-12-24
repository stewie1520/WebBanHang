using System;

using WebBanHang.Models;
using WebBanHang.DTOs.Commons;

namespace WebBanHang.Commons
{
  public class PaginationHelper
  {
    public static Pagination CreatePagination(PaginationParam paginationParam, int totalRow)
    {
      return new Pagination()
      {
        CurrentPage = paginationParam.Page,
        TotalPage = (int)totalRow / paginationParam.PerPage + 1,
        Count = totalRow
      };
    }
  }
}
