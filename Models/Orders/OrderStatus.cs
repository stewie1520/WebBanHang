namespace WebBanHang.Models
{
    public enum OrderStatus
    {
        Ordering,
        UserConfirm,
        ShopConfirm,
        UserReject,
        Packing,
        WaitingShipper,
        ShipperPicked,
        Delivering,
        Delivered,
        Returned,
        Cancel 
    }
}
