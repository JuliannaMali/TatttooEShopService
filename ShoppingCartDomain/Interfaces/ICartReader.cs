using ShoppingCartDomain.Models;

namespace ShoppingCartDomain.Interfaces;
public interface ICartReader
{
    Cart GetCart(int cartId);
    List<Cart> GetAllCarts();
}