using System;
using System.Threading.Tasks;
using Convey;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Identity.Application.Commands;
using Pacco.Services.Identity.Application.Services;
using Pacco.Services.Identity.Infrastructure;

namespace Pacco.Services.Identity
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddInfrastructureModule())
                .Configure(app => app
                    .UseErrorHandler()
                    .UseAuthentication()
                    .UsePublicContracts(false)
                    .UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync("Welcome to Pacco Identity Service!"))
                        .Get("me", async ctx =>
                        {
                            var result = await ctx.AuthenticateAsync("Bearer");
                            if (!result.Succeeded)
                            {
                                ctx.Response.StatusCode = 401;
                                return;
                            }

                            var userId = Guid.Parse(result.Principal.Identity.Name);
                            var user = await ctx.RequestServices.GetService<IIdentityService>().GetAsync(userId);
                            if (user is null)
                            {
                                ctx.Response.StatusCode = 404;
                                return;
                            }

                            ctx.Response.WriteJson(user);
                        })
                        .Post<SignIn>("sign-in", async (req, ctx) =>
                        {
                            var token = await ctx.RequestServices.GetService<IIdentityService>().SignInAsync(req);
                            ctx.Response.WriteJson(token);
                        })
                        .Post<SignUp>("sign-up", async (req, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().SignUpAsync(req);
                            await ctx.Response.NoContent();
                        })
                    ))
                .Build()
                .RunAsync();
    }
}
