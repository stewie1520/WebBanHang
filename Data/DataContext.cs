using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace WebBanHang.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
    }
}
