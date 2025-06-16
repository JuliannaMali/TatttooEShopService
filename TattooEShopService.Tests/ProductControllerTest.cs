using Microsoft.AspNetCore.Mvc;
using Moq;
using TattooEShopApplication.Services;
using TattooEShopDomain.Models;
using TatttooEShopService.Controllers;

namespace TattooEShopService.Tests;

public class ProductControllerTest
{
    private readonly Mock<IProductService> _mock;
    private readonly ProductController _controller;

    public ProductControllerTest()
    {
        _mock = new Mock<IProductService>();
        _controller = new ProductController( _mock.Object );
    }

    [Fact]
    public async Task Get_ShouldReturnAllProducts_ResultTrue()
    {
        var products = new List<Product> { new Product(), new Product(), new Product() };
        _mock.Setup(t => t.GetAllAsync()).ReturnsAsync( products );

        var result = await _controller.Get();

        var ok = Assert.IsType<OkObjectResult>( result );

        Assert.Equal(products, ok.Value);
    }

    [Fact]
    public async Task GetById_ReturnsProduct_ResultTrue()
    {
        var product = new Product{  Id = 1  };
        _mock.Setup(t => t.GetAsync(1)).ReturnsAsync( product );

        var result = await _controller.Get(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(product, ok.Value);
    }

    [Fact]
    public async Task GetById_InvalidId_ResultNotFound()
    {
        var product = new Product { Id = 1 };
        _mock.Setup(t => t.GetAsync(1)).ReturnsAsync(product);

        var result = await _controller.Get(2);

        Assert.IsType<NotFoundResult>(result);
    }
}
