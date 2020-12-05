using WebBanHang.Services;

namespace WebBanHang.DTOs.Commons
{
    public class PaginationParam
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = BaseService.DefaultPerPage;
    }
}
