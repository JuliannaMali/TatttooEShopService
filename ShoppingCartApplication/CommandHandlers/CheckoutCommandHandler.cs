using MediatR;
using ShoppingCartDomain.Commands;
using ShoppingCartDomain.Interfaces;
using ShoppingCartDomain.Models;
using ShoppingCartInfrastructure.Kafka;
using System.Text.Json;

namespace ShoppingCartApplication.CommandHandlers;

public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand>
{
    private readonly ICartReader _cartReader;
    private readonly IOrderRepository _orderRepository;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly IProductInfoService _productInfoService;

    public CheckoutCommandHandler(
        ICartReader cartReader,
        IOrderRepository orderRepository,
        IKafkaProducer kafkaProducer,
        IProductInfoService productInfoService)
    {
        _cartReader = cartReader;
        _orderRepository = orderRepository;
        _kafkaProducer = kafkaProducer;
        _productInfoService = productInfoService;
    }

    public async Task Handle(CheckoutCommand command, CancellationToken cancellationToken)
    {
        var cart = _cartReader.GetCart(command.CartId);
        if (cart == null || !cart.Products.Any()) return;

        var items = new List<OrderItem>();
        foreach (var p in cart.Products)
        {
            var product = await _productInfoService.GetProductAsync(p.Id);

            items.Add(new OrderItem
            {
                ProductId = p.Id,
                ProductName = product.Name,
                UnitPrice = product.Price
            });
        }

        var total = items.Sum(i => i.UnitPrice);
        var order = new Order
        {
            UserId = cart.Id,
            Items = items,
            TotalPrice = total
        };

        _orderRepository.Add(order);

        var receiptText = $"ORDER #{order.Id}\n" +
                          $"UserId: {order.UserId}\n" +
                          $"Date: {order.CreatedAt}\n\n" +
                          $"Items:\n" +
                          string.Join("\n", order.Items.Select(i => $"- {i.ProductName} ({i.ProductId}) - {i.UnitPrice} zł")) +
                          $"\n\nTotal: {order.TotalPrice} zł";

        Directory.CreateDirectory("receipts");
        File.WriteAllText($"receipts/order_{order.Id}.txt", receiptText);

        await _kafkaProducer.SendMessageAsync("checkout-topic", JsonSerializer.Serialize(order));
    }
}
