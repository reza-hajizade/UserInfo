using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UserInfo.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;

namespace UserInfo.Infrastructure.Extensions;



public static class ApplicationExtensions
{
    // Adds DbContext, MassTransit, services, and validators to the container

    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<UserInfoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SvcDbContext")));


        builder.Services.AddMassTransit(configure =>
        {
            var brokerConfig = builder.Configuration.GetSection(BrokerOptions.SectionName)
                                                    .Get<BrokerOptions>();
            if (brokerConfig is null)
            {
                throw new ArgumentNullException(nameof(BrokerOptions));
            }

            configure.AddConsumers(Assembly.GetExecutingAssembly());
            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(brokerConfig.Host, hostConfigure =>
                {
                    hostConfigure.Username(brokerConfig.Username);
                    hostConfigure.Password(brokerConfig.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });



        builder.Services.AddScoped<UserInfoServices>();


        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

