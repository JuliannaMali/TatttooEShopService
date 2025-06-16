using ShoppingCartDomain.Models;

namespace ShoppingCartDomain.Interfaces;
public interface ICartAdder
{
    void AddProductToCart(int cartId, Product product);
}
