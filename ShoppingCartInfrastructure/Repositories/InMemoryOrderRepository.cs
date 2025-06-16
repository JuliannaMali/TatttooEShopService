using ShoppingCartDomain.Interfaces;
using ShoppingCartDomain.Models;

namespace ShoppingCartInfrastructure.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    public void Add(Order order)
    {
        order.Id = _orders.Count + 1;
        _orders.Add(order);
    }

    public List<Order> GetAll() => _orders;
}
