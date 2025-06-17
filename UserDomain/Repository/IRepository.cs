using UserDomain.Models.DTO;

namespace UserDomain.Repository;

public interface IRepository
{
    Task<Models.Entities.User> AddClientAsync(UserCreateDTO userDto);
    Task<Models.Entities.User> AddEmployeeAsync(UserCreateDTO userDto);
    Task<Models.Entities.User> AddAdminAsync(UserCreateDTO userDto);

    Task<Models.Entities.User> UpdateAsync(int userId, UserUpdateDTO userDto);

    Task<Models.Entities.User> DeleteAsync(Models.Entities.User user);
}
