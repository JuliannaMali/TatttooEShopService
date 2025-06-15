using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserDomain.Models.DTO;
using UserDomain.Repository;

namespace UserApplication.Services.User;

public class UserService : IUserService
{
    private readonly IMapper _mapper;

    private readonly UserDomain.Repository.DbContext context;

    private IRepository _repository;

    public UserService(IMapper mapper, UserDomain.Repository.DbContext dbContext, IRepository repository)
    {
        _mapper = mapper;
        context = dbContext;
        _repository = repository;
    }

    public UserResponseDTO GetUser(int userId)
    {
        var user = context.Users.Find(userId);

        if (user == null)
            throw new Exception("Record not found");

        return _mapper.Map<UserResponseDTO>(user);
    }


    public async Task<UserDomain.Models.Entities.User> AddClient(UserCreateDTO user)
    {
        var result = await _repository.AddClientAsync(user);
        return result;
    }
    public async Task<UserDomain.Models.Entities.User> AddEmployee(UserCreateDTO user)
    {
        var result = await _repository.AddEmployeeAsync(user);
        return result;
    }
    public async Task<UserDomain.Models.Entities.User> AddAdmin(UserCreateDTO user)
    {
        var result = await _repository.AddAdminAsync(user);
        return result;
    }

    public async Task<UserDomain.Models.Entities.User> Update(int userId)
    {
        var user = context.Users.Find(userId);

        var userDto = new UserUpdateDTO
        {
            Email = user.Email,
            PasswordHash = user.PasswordHash,
        };
        var result = await _repository.UpdateAsync(userDto);
        return result;
    }
    public async Task<UserDomain.Models.Entities.User> Delete(int userId)
    {
        var user = context.Users.Find(userId);
        var result = await _repository.DeleteAsync(user!);
        return result;
    }
}
