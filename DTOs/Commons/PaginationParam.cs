using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebBanHang.Services;

namespace WebBanHang.DTOs.Commons
{
  public class PaginationParam : IValidatableObject
  {
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = BaseService.DefaultPerPage;

    public string SortBy { get; set; }

    public OrderByType OrderBy { get; set; } = OrderByType.ASC;

    public enum OrderByType
    {
      ASC,
      DESC,
    }

    public int Skip() => (Page - 1) * PerPage;
    public int TotalPage(long totalPage) => (int)Math.Ceiling(1.0m * totalPage / PerPage);
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (PerPage < 0)
      {
        yield return new ValidationResult(
          errorMessage: "Số lượng hiển thị phải lớn hơn 0",
          memberNames: new[] { nameof(PerPage) }
        );
      }

      if (Page < 1)
      {
        yield return new ValidationResult(
          errorMessage: "Số page không hợp lệ",
          memberNames: new[] { nameof(Page) }
        );
      }
    }
  }
}
