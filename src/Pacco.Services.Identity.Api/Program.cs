using System;
using System.Threading.Tasks;
using Convey;
using Convey.Auth;
using Convey.Secrets.Vault;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Identity.Application;
using Pacco.Services.Identity.Application.Commands;
using Pacco.Services.Identity.Application.Queries;
using Pacco.Services.Identity.Application.Services;
using Pacco.Services.Identity.Infrastructure;

namespace Pacco.Services.Identity.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetUser>("users/{userId}", (query, ctx) => GetUserAsync(query.UserId, ctx))
                        .Get("me", async ctx =>
                        {
                            var userId = await ctx.AuthenticateUsingJwtAsync();
                            if (userId == Guid.Empty)
                            {
                                ctx.Response.StatusCode = 401;
                                return;
                            }

                            await GetUserAsync(userId, ctx);
                        })
                        .Post<SignIn>("sign-in", async (cmd, ctx) =>
                        {
                            var token = await ctx.RequestServices.GetService<IIdentityService>().SignInAsync(cmd);
                            await ctx.Response.WriteJsonAsync(token);
                        })
                        .Post<SignUp>("sign-up", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().SignUpAsync(cmd);
                            await ctx.Response.Created("identity/me");
                        })
                        .Post<RevokeAccessToken>("access-tokens/revoke", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IAccessTokenService>().DeactivateAsync(cmd.AccessToken);
                            ctx.Response.StatusCode = 204;
                        })
                        .Post<UseRefreshToken>("refresh-tokens/use", async (cmd, ctx) =>
                        {
                            var token = await ctx.RequestServices.GetService<IRefreshTokenService>().UseAsync(cmd.RefreshToken);
                            await ctx.Response.WriteJsonAsync(token);
                        })
                        .Post<RevokeRefreshToken>("refresh-tokens/revoke", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IRefreshTokenService>().RevokeAsync(cmd.RefreshToken);
                            ctx.Response.StatusCode = 204;
                        })
                    ))
                .UseLogging()
                .UseVault()
                .Build()
                .RunAsync();

        private static async Task GetUserAsync(Guid id, HttpContext context)
        {
            var user = await context.RequestServices.GetService<IIdentityService>().GetAsync(id);
            if (user is null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            await context.Response.WriteJsonAsync(user);
        }
    }
}
