using Microsoft.EntityFrameworkCore;
using UserInfo.Models;

namespace UserInfo.Infrastructure;

public class UserInfoDbContext(DbContextOptions<UserInfoDbContext> dbContextOptionsoptions)
    : DbContext(dbContextOptionsoptions)
{
    public DbSet<User> Users { get; set; }

}


