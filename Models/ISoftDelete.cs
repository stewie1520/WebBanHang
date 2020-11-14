using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
