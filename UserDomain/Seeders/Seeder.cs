using System.Text;
using System.Security.Cryptography;
using UserDomain.Models.Entities;

namespace UserDomain.Seeders;

public class Seeder (Repository.DbContext context) : ISeeder
{
    public async Task Seed()
    {
        if (!context.Users.Any())
        {
            var user = new User
            {
                Username = "admin",
                Email = "admin@gmail.com",
                PasswordHash = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("adminpass"))).Replace("-", "").ToLower(),
            };
            context.Users.AddRange(user);
            context.SaveChanges();

        }
        if (!context.Roles.Any())
        {
            var admin = context.Users.First(c => c.Username == "admin");
            var roles = new List<Role>
            {
                new Role { Name = "Administrator" , UserId = admin.UserId},
                new Role { Name = "Employee", UserId = admin.UserId },
                new Role { Name = "Client", UserId = admin.UserId },
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
    }
}
