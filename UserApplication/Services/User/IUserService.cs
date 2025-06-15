using UserDomain.Models.DTO;
using UserDomain.Models.Entities;

namespace UserApplication.Services.User;

public interface IUserService
{
    UserResponseDTO GetUser(int id);

    Task<UserDomain.Models.Entities.User> AddClient(UserCreateDTO user);
    Task<UserDomain.Models.Entities.User> AddEmployee(UserCreateDTO user);
    Task<UserDomain.Models.Entities.User> AddAdmin(UserCreateDTO user);

    Task<UserDomain.Models.Entities.User> Update(int userId, UserUpdateDTO user);

    Task<UserDomain.Models.Entities.User> Delete(int userId);
}