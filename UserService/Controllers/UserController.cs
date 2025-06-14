using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserDomain.Models.DTO;
using UserApplication.Services.User;

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
    //[Authorize(Policy ="AdminOnly")]
    //[Authorize(Policy ="EmployeeOnly")]
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

    [HttpPost("AddClient")]
    public async Task<ActionResult> AddClient([FromBody] UserCreateDTO user)
    {
        var result = await _userService.AddClient(user);
        return Ok(result);
    }

    [HttpPost("AddEmployee")]
    public async Task<ActionResult> AddEmployee([FromBody] UserCreateDTO user)
    {
        var result = await _userService.AddEmployee(user);
        return Ok(result);
    }

    [HttpPost("AddAdmin")]
    public async Task<ActionResult> AddAdmin([FromBody] UserCreateDTO user)
    {
        var result = await _userService.AddAdmin(user);
        return Ok(result);
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> Update(int id)
    {
        var result = await _userService.Update(id);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _userService.Delete(id);
        return Ok(result);
    }

}