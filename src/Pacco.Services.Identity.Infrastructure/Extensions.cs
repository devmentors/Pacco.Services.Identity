using System;
using System.Threading.Tasks;
using Convey;
using Convey.Auth;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Identity.Application;
using Pacco.Services.Identity.Application.Commands;
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

            return builder
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddJwt()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddRabbitMq(plugins: p => p.RegisterJaeger())
                .AddMongo()
                .AddMetrics()
                .AddJaeger()
                .AddMongoRepository<UserDocument, Guid>("Users");
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseJaeger()
                .UseInitializers()
                .UseMongo()
                .UsePublicContracts<ContractAttribute>()
                .UseConsul()
                .UseMetrics()
                .UseAuthentication()
                .UseRabbitMq()
                .SubscribeCommand<SignUp>();

            return app;
        }

        public static async Task<Guid> AuthenticateUsingJwtAsync(this HttpContext context)
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