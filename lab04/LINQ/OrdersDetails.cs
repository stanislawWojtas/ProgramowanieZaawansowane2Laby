namespace linq;
public class OrderDetails
{
    public string? OrderId;
    public string? ProductId;
    public string? UnitPrice;
    public string? Quantity;
    public string? Discount;

    public OrderDetails(string? orderId, string? productId, string? unitPrice, string? quantity, string? discount)
    {
        OrderId = orderId;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Discount = discount;
    }
}
