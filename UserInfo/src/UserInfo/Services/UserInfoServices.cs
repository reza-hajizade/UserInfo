using UserInfo.Infrastructure;

namespace UserInfo.Services
{
    public sealed class UserInfoServices(UserInfoDbContext context)
    {
       public UserInfoDbContext Context { get; } = context; 
    }
    
    
}
