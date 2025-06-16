using MediatR;
using ShoppingCartDomain.Interfaces;
using ShoppingCartDomain.Models;
using ShoppingCartDomain.Queries;

namespace ShoppingCartApplication.QueryHandlers;
public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Cart>
{
    private readonly ICartReader _cartReader;

    public GetCartQueryHandler(ICartReader cartReader)
    {
        _cartReader = cartReader;
    }

    public Task<Cart> Handle(GetCartQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(_cartReader.GetCart(query.CartId));
    }
}