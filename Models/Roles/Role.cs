using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models
{
    public class Role : BaseModel, ISoftDelete
    {
        public bool IsDeleted { get; set; } = false;
        public string RoleName { get; set; }
        public string Description { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
