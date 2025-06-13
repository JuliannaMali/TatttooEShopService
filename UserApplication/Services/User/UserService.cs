using AutoMapper;
using UserDomain.Models.Response;
using UserService.Controllers;

namespace UserApplication.Services.User;

public class UserService : IUserService
{
    private readonly IMapper _mapper;

    private readonly TattooEShopDomain.Repository.DbContext context;

    public UserService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public UserResponseDTO GetUser(int userId)
    {
        var user = context.Users.Find(userId);

        if (user == null)
            throw new Exception("Record not found");

        return _mapper.Map<UserResponseDTO>(user);
    }
}
