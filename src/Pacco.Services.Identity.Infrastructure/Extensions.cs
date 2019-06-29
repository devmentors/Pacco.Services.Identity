using System;
using System.Threading.Tasks;
using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Identity.Application;
using Pacco.Services.Identity.Application.Services;
using Pacco.Services.Identity.Application.Services.Identity;
using Pacco.Services.Identity.Core.Repositories;
using Pacco.Services.Identity.Infrastructure.Auth;
using Pacco.Services.Identity.Infrastructure.MessageBrokers;
using Pacco.Services.Identity.Infrastructure.Mongo;
using Pacco.Services.Identity.Infrastructure.Mongo.Documents;
using Pacco.Services.Identity.Infrastructure.Mongo.Repositories;

namespace Pacco.Services.Identity.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
            builder.Services.AddSingleton<IIdentityService, IdentityService>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IPasswordHasher<IPasswordService>, PasswordHasher<IPasswordService>>();

            return builder.AddJwt()
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddQueryHandlers()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddRabbitMq()
                .AddMongo()
                .AddMongoRepository<UserDocument, Guid>("Users");
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UsePublicContracts<ContractAttribute>()
                .UseInitializers()
                .UseConsul()
                .UseMongo()
                .UseAuthentication()
                .UsePublicContracts(false)
                .UseRabbitMq();

            return app;
        }

        public static async Task<Guid> JwtAuthAsync(this HttpContext context)
        {
            var authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (authentication.Succeeded)
            {
                return Guid.Parse(authentication.Principal.Identity.Name);
            }
            
            context.Response.StatusCode = 401;
            return Guid.Empty;
        }
    }
}