using UserDomain.Models.Response;

namespace UserService.Controllers;

public interface IUserService
{
    UserResponseDTO GetUser(int id);
}