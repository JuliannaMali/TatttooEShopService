namespace UserApplication.Services.JWT;

public interface IJwtTokenService
{
    string GenerateToken(int userId, List<string> roles);
}
