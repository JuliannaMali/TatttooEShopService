using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TatttooEShopService;
using TattooEShopDomain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TattooEShopDomain.Models;
using System.Net.Http.Json;

namespace TattooEShopIntegrationTests;

public class ProductControllerIntTest
{
    private readonly HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    public ProductControllerIntTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));

                    services.Remove(dbContextOptions);

                    services
                        .AddDbContext<DataContext>(options => options.UseInMemoryDatabase("TestDB"));
                });
            });


        _client = _factory.CreateClient();
    }


    [Fact]
    public async Task Get_ReturnsAll_ExpectedThreeProducts()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            context.Products.RemoveRange(context.Products);

            context.Products.AddRange(
                new Product { Name = "Ink" },
                new Product { Name = "Skin" },
                new Product { Name = "Machine" }
            );

            await context.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/api/product");

        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<List<Product>>();

        Assert.Equal(3, products?.Count);
    }

}
