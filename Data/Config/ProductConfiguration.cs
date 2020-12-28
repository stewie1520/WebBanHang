using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

using WebBanHang.Models;

namespace WebBanHang.Data.Config
{
  public class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.HasIndex(x => new { x.Name });
      builder.Property(x => x.Tags)
        .HasConversion(
          v => string.Join(',', v),
          v => new List<string>(v.Split(',', StringSplitOptions.RemoveEmptyEntries))
        );
      builder.Property(x => x.Features)
        .HasConversion(
          v => string.Join(@"\|/", v),
          v => new List<string>(v.Split(@"\|/", StringSplitOptions.RemoveEmptyEntries))
        );
    }
  }
}
