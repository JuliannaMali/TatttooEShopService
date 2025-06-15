using System.Text;
using System.Security.Cryptography;
using UserApplication.Services.JWT;
using UserDomain.Exceptions;
using Microsoft.EntityFrameworkCore;
using UserApplication.Producer;

namespace UserApplication.Services.Login;

public class LoginService : ILoginService
{
    protected IJwtTokenService _jwtTokenService;
    private readonly UserDomain.Repository.DbContext _dbContext;
    protected IKafkaProducer _kafkaProducer;


    public LoginService(IJwtTokenService jwtTokenService, UserDomain.Repository.DbContext context, IKafkaProducer kafkaProducer)
    {
        _jwtTokenService = jwtTokenService;
        _dbContext = context;
        _kafkaProducer = kafkaProducer;
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
            await _kafkaProducer.SendMessageAsync("after-login-email-topic", user.Email.ToString());

            return token;
        }
    }
}
