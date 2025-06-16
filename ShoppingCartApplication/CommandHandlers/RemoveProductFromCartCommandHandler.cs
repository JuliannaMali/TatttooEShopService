using MediatR;
using ShoppingCartDomain.Commands;
using ShoppingCartDomain.Interfaces;

namespace ShoppingCartApplication.CommandHandlers;

public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand>
{
    private readonly ICartRemover _cartRemover;

    public RemoveProductFromCartCommandHandler(ICartRemover cartRemover)
    {
        _cartRemover = cartRemover;
    }

    public Task Handle(RemoveProductFromCartCommand command, CancellationToken cancellationToken)
    {
        _cartRemover.RemoveProductFromCart(command.CartId, command.ProductId);
        return Task.CompletedTask;
    }
}