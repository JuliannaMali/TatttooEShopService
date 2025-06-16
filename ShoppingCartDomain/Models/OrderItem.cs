namespace ShoppingCartDomain.Models;
public class OrderItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
}