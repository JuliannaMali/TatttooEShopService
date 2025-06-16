using ShoppingCartDomain.Models;

namespace ShoppingCartInfrastructure.Repositories;

public interface ICartRepository
{
    void Add(Cart cart);
    void Update(Cart cart);
    Cart FindById(int id);
    List<Cart> GetAll();
}