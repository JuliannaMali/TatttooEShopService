using Microsoft.AspNetCore.Mvc;
using UserApplication.Services.Login;
using UserDomain.Exceptions;
using UserDomain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    protected ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }


    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = _loginService.Login(request.Username, request.Password);
            return Ok(new { token });
        }
        catch (InvalidCredentialsException)
        {
            return Unauthorized();
        }
    }

}
