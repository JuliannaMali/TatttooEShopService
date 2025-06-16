namespace ShoppingCartDomain.Interfaces;
public interface IProductInfoService
{
    Task<(string Name, decimal Price)> GetProductAsync(int productId);
}
