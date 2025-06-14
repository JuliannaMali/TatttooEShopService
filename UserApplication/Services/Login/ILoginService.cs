namespace UserApplication.Services.Login;

public interface ILoginService
{
    Task<string> Login(string username, string password);
}
