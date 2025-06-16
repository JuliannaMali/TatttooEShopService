using MediatR;
using ShoppingCartDomain.Interfaces;
using ShoppingCartDomain.Models;
using ShoppingCartDomain.Queries;

namespace ShoppingCartApplication.QueryHandlers;

public class GetAllCartsQueryHandler : IRequestHandler<GetAllCartsQuery, List<Cart>>
{
    private readonly ICartReader _cartReader;

    public GetAllCartsQueryHandler(ICartReader cartReader)
    {
        _cartReader = cartReader;
    }

    public Task<List<Cart>> Handle(GetAllCartsQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(_cartReader.GetAllCarts());
    }
}