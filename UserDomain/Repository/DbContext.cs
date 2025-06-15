using Microsoft.EntityFrameworkCore;
using UserDomain.Models.Entities;

namespace UserDomain.Repository;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions<DbContext> options) : base(options) { }

    public DbSet<UserDomain.Models.Entities.User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}
