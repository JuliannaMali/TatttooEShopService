using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserApplication.Services.User;
using UserDomain.Models.DTO;
using UserDomain.Models.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Policy = "Managerial")]
    public ActionResult<UserResponseDTO> GetUserData()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        try
        {
            var userDto = _userService.GetUser(userId);
            return Ok(userDto);
        }
        catch
        {
            return NotFound();
        }

    }

    [Authorize(Policy = "LoggedIn")]
    [HttpPost("AddClient")]
    public async Task<ActionResult> AddClient([FromBody] UserCreateDTO user)
    {
        var result = await _userService.AddClient(user);
        return Ok(result);
    }

    [Authorize(Policy = "Managerial")]
    [HttpPost("AddEmployee")]
    public async Task<ActionResult> AddEmployee([FromBody] UserCreateDTO user)
    {
        var result = await _userService.AddEmployee(user);
        return Ok(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("AddAdmin")]
    public async Task<ActionResult> AddAdmin([FromBody] UserCreateDTO user)
    {
        var result = await _userService.AddAdmin(user);
        return Ok(result);
    }

    [Authorize(Policy = "LoggedIn")]
    [HttpPut("Update")]
    public async Task<ActionResult> Update([FromBody] UserUpdateDTO user)
    {
        var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _userService.Update(id, user);
        return Ok(result);
    }

    [Authorize(Policy = "LoggedIn")]
    [HttpDelete("Delete")]
    public async Task<ActionResult> Delete()
    {
        var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _userService.Delete(id);
        return Ok(result);
    }

}