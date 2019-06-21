using System;
using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Identity.Application;
using Pacco.Services.Identity.Application.Services;
using Pacco.Services.Identity.Core.Entities;
using Pacco.Services.Identity.Core.Repositories;
using Pacco.Services.Identity.Core.Services;
using Pacco.Services.Identity.Infrastructure.Auth;
using Pacco.Services.Identity.Infrastructure.MessageBrokers;
using Pacco.Services.Identity.Infrastructure.Mongo.Repositories;

namespace Pacco.Services.Identity.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructureModule(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IIdentityService, IdentityService>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IPasswordHasher<IPasswordService>, PasswordHasher<IPasswordService>>();

            return builder.AddJwt()
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddQueryHandlers()
                .AddRabbitMq()
                .AddMongo()
                .AddMongoRepository<RefreshToken, Guid>("refreshTokens")
                .AddMongoRepository<User, Guid>("users");
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseInitializers().UseRabbitMq();

            return app;
        }
    }
}