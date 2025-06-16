using MediatR;
using ShoppingCartDomain.Models;

namespace ShoppingCartDomain.Queries;
public class GetCartQuery : IRequest<Cart>
{
    public int CartId { get; set; }
}
