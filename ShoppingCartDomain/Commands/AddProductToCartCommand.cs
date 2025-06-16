using MediatR;

namespace ShoppingCartDomain.Commands;
public class AddProductToCartCommand : IRequest
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
}
