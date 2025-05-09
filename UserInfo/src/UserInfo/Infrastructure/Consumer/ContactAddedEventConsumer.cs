using MassTransit;
using Microsoft.EntityFrameworkCore;
using UserInfo.Infrastructure.Consumer.IntegrationEvents;
using UserInfo.Models;

namespace UserInfo.Infrastructure.Consumer
{
    // Handles ContactAddedEvent: adds phone number to the related user

    public class ContactAddedEventConsumer(UserInfoDbContext userInfoDbContext) : IConsumer<ContactAddedEvent>
    {
        private readonly UserInfoDbContext _userInfoDbContext = userInfoDbContext;
        public async Task Consume(ConsumeContext<ContactAddedEvent> context)
        {

            var user = await _userInfoDbContext.Users
                .FirstOrDefaultAsync(x => x.UserId == context.Message.UserId);

            if (user is null)
            {
                Console.WriteLine("User not found");
                 return;
            }

            user.CreateContactUserMessage(context.Message.phoneNumber);

            await _userInfoDbContext.SaveChangesAsync();
        }
    
    }
}
