using MediatR;

namespace ShoppingCartDomain.Commands;

public class CheckoutCommand : IRequest
{
    public int CartId { get; set; }
}