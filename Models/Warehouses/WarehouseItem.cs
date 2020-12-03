using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models
{
    public class WarehouseItem : BaseModel, ISoftDelete
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double AverageCost { get; set; }
        // TODO: will fix later
        // public WarehouseImport LastImport { get; set; }
        // public WarehouseExport LastExport { get; set; }
        // public WarehouseAdjustment LastAdjustment { get; set; }
        public bool IsDeleted { get; set; }
    }
}
