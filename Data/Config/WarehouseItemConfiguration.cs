using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBanHang.Models;

namespace WebBanHang.Data.Config
{
    public class WarehouseItemConfiguration : IEntityTypeConfiguration<WarehouseItem>
    {
        public void Configure(EntityTypeBuilder<WarehouseItem> builder)
        {
            builder.HasIndex("ProductId");
        }
    }
}
