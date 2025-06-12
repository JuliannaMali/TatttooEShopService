using UserApplication.Services.JWT;
using UserDomain;

namespace UserApplication.Services.Login;

public class LoginService : ILoginService
{
    protected IJwtTokenService _jwtTokenService;

    public LoginService(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    public string Login(string username, string password)
    {
        if (username == "admin" && password == "password")
        {
            var roles = new List<string> { "Client", "Employee", "Administrator" };
            var token = _jwtTokenService.GenerateToken(123, roles);
            return token;
        }
        else
        {
            throw new InvalidCredentialsException();
        }

    }
}
