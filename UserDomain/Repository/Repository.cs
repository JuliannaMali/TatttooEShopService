using System.Text;
using System.Security.Cryptography;
using UserDomain.Models.DTO;

namespace UserDomain.Repository;

public class Repository : IRepository
{
    private readonly UserDomain.Repository.DbContext _dbContext;

    public Repository(UserDomain.Repository.DbContext datacontext)
    {
        _dbContext = datacontext;
    }


    public async Task<Models.Entities.User> AddClientAsync(UserCreateDTO userDto)
    {
        var user = new UserDomain.Models.Entities.User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(userDto.Password))).Replace("-", "").ToLower()

        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        _dbContext.Roles.Add(new Models.Entities.Role { Name = "Client", UserId = user.UserId });

        await _dbContext.SaveChangesAsync();
        return user;
    }
    public async Task<Models.Entities.User> AddEmployeeAsync(UserCreateDTO userDto)
    {
        var user = new Models.Entities.User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(userDto.Password))).Replace("-", "").ToLower()
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        _dbContext.Roles.Add(new Models.Entities.Role { Name = "Client", UserId = user.UserId });
        _dbContext.Roles.Add(new Models.Entities.Role { Name = "Employee", UserId = user.UserId });

        await _dbContext.SaveChangesAsync();
        return user;
    }
    public async Task<Models.Entities.User> AddAdminAsync(UserCreateDTO userDto)
    {
        var user = new Models.Entities.User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(userDto.Password))).Replace("-", "").ToLower()
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        _dbContext.Roles.Add(new Models.Entities.Role { Name = "Client", UserId = user.UserId });
        _dbContext.Roles.Add(new Models.Entities.Role { Name = "Employee", UserId = user.UserId });
        _dbContext.Roles.Add(new Models.Entities.Role { Name = "Administrator", UserId = user.UserId });

        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<Models.Entities.User> UpdateAsync(int userId, UserUpdateDTO userDto)
    {
        var newemail = userDto.Email;
        var newpassword = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(userDto.Password))).Replace("-", "").ToLower();

        var user = _dbContext.Users.Find(userId);

        user.Email = newemail;
        user.PasswordHash = newpassword;
         
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<Models.Entities.User> DeleteAsync(Models.Entities.User user)
    {
        var roles = _dbContext.Roles.Where(r => r.UserId == user.UserId).ToList();

        for(int i = 0; i < roles.Count; i++)
        {
            var remove = _dbContext.Roles.Find(roles[i].Id);
            _dbContext.Roles.Remove(remove!);
        }
        await _dbContext.SaveChangesAsync();

        
        _dbContext.Users.Remove(user);

        await _dbContext.SaveChangesAsync();
        return user;
    }
}
