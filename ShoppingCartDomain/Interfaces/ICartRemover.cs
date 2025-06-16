namespace ShoppingCartDomain.Interfaces;
public interface ICartRemover
{
    void RemoveProductFromCart(int cartId, int productId);
}
