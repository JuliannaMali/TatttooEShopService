using ShoppingCartDomain.Interfaces;
using System.Net.Http.Json;

namespace ShoppingCartInfrastructure.Services;

public class ProductInfoService : IProductInfoService
{
    private readonly HttpClient _client;

    public ProductInfoService(HttpClient client)
    {
        _client = client;
    }

    public async Task<(string Name, decimal Price)> GetProductAsync(int productId)
    {
        var result = await _client.GetFromJsonAsync<ProductDto>($"api/product/{productId}");
        return (result?.Name ?? "Unknown", result?.Price ?? 0);
    }

    private class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
