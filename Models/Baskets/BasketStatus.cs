namespace WebBanHang.Models
{
    public enum BasketStatus
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
