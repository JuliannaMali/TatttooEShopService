using System.Text;
using System.Security.Cryptography;
using UserApplication.Services.JWT;
using UserDomain.Exceptions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UserApplication.Services.Login;

public class LoginService : ILoginService
{
    protected IJwtTokenService _jwtTokenService;
    private readonly UserDomain.Repository.DbContext _dbContext;

    public LoginService(IJwtTokenService jwtTokenService, UserDomain.Repository.DbContext context)
    {   
        _jwtTokenService = jwtTokenService;
        _dbContext = context;
    }


    public async Task<string> Login(string username, string password)
    {

        string hash = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "").ToLower();

        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hash);


        if (user == null)
        {
            throw new InvalidCredentialsException();
        }
        else
        {

            var roleList = await _dbContext.Roles.Where(r => r.UserId == user.UserId).Select(r => r.Name).ToListAsync();
            var roles = new List<string> { "Client" };

            if (roleList.Contains("Employee"))
                roles.Add("Employee");

            if (roleList.Contains("Administrator"))
                roles.Add("Administrator");



            var token = _jwtTokenService.GenerateToken(user.UserId, roles);
            return token;

        }

        //if (username == "admin" && password == "password")
        //{
        //    var roles = new List<string> { "Client", "Employee", "Administrator" };
        //    var token = _jwtTokenService.GenerateToken(123, roles);
        //    return token;
        //}
        //else
        //{
        //    throw new InvalidCredentialsException();
        //}

    }
}
