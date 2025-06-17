using Microsoft.AspNetCore.Mvc;
using ShoppingCartDomain.Commands;
using MediatR;
using ShoppingCartDomain.Queries;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UserDomain.Models.Entities;
using ShoppingCartInfrastructure.Kafka;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCartService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserDomain.Repository.DbContext _dbcontext;
    protected IKafkaProducer _kafkaProducer;
    public CartController(IMediator mediator, UserDomain.Repository.DbContext context, IKafkaProducer kafkaProducer)
    {
        _mediator = mediator;
        _dbcontext = context;
        _kafkaProducer = kafkaProducer;
    }

    //[HttpPost("add-product")]
    //public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartCommand command)
    //{
    //    await _mediator.Send(command);
    //    return Ok();
    //}

    [Authorize(Policy = "LoggedIn")]
    [HttpPost("AddProduct{productId}")]
    public async Task<IActionResult> AddProductToCart(int productId)
    {
        var cartId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var command = new AddProductToCartCommand
        {
            CartId = cartId,
            ProductId = productId
        };
        await _mediator.Send(command);
        return Ok();
    }

    [Authorize(Policy = "LoggedIn")]
    [HttpPost("RemoveProduct{productId}")]
    public async Task<IActionResult> RemoveProductToCart(int productId)
    {
        var cartId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var command = new RemoveProductFromCartCommand
        {
            CartId = cartId,
            ProductId = productId
        };
        await _mediator.Send(command);
        return Ok();
    }

    //[HttpPost("remove-product")]
    //public async Task<IActionResult> RemoveProductFromCart([FromBody] RemoveProductFromCartCommand command)
    //{
    //    await _mediator.Send(command);
    //    return Ok();
    //}

    [Authorize(Policy = "LoggedIn")]
    [HttpGet("GetLoggedUserCart")]
    public async Task<IActionResult> GetCart()
    {
        var cartId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var query = new GetCartQuery { CartId = cartId };
        var result = await _mediator.Send(query);
        return result == null ? NotFound() : Ok(result);
    }

    [Authorize(Policy = "Managerial")]
    [HttpGet("GetAllCarts")]
    public async Task<IActionResult> GetAllCarts()
    {
        var query = new GetAllCartsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize(Policy = "LoggedIn")]
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var user = _dbcontext.Users.Find(userId);
        var command = new CheckoutCommand { CartId = userId }; // zakładamy CartId = UserId

        if (user == null)
            return NotFound("User not found");

        await _kafkaProducer.SendMessageAsync("checkout-topic", user.Email.ToString()); 

        await _mediator.Send(command);
        return Ok("Checkout complete");
    }

}
