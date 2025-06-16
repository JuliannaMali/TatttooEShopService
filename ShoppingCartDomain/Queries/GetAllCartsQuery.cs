using MediatR;
using ShoppingCartDomain.Models;

namespace ShoppingCartDomain.Queries;
public class GetAllCartsQuery : IRequest<List<Cart>>
{
}
