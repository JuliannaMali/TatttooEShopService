using System.Text;
using System.Security.Cryptography;
using UserApplication.Services.JWT;
using UserDomain.Exceptions;

namespace UserApplication.Services.Login;

public class LoginService : ILoginService
{
    protected IJwtTokenService _jwtTokenService;
    private readonly TattooEShopDomain.Repository.DbContext _dbContext;

    public LoginService(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    public string Login(string username, string password)
    {

        string hash = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "").ToLower();

        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hash);


        //if (user == null)
        //{
        //    throw new InvalidCredentialsException();
        //}
        //else
        //{
        //    if (user.Roles.Contains(new UserDomain.Models.Entities.Role()))
        //}

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
