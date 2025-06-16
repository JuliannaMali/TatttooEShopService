using ShoppingCartDomain.Models;

namespace ShoppingCartDomain.Interfaces;
public interface IOrderRepository
{
    public void Add(Order order);
    public List<Order> GetAll();
}
