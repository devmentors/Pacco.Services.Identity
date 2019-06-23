using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Identity.Application;
using Pacco.Services.Identity.Application.Commands;
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
                    .UseErrorHandler()
                    .UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync("Welcome to Pacco Identity Service!"))
                        .Get("me", async ctx =>
                        {
                            var userId = await ctx.JwtAuthAsync();
                            var user = await ctx.RequestServices.GetService<IIdentityService>().GetAsync(userId);
                            if (user is null)
                            {
                                ctx.Response.StatusCode = 404;
                                return;
                            }

                            ctx.Response.WriteJson(user);
                        })
                        .Post<SignIn>("sign-in", async (cmd, ctx) =>
                        {
                            var token = await ctx.RequestServices.GetService<IIdentityService>().SignInAsync(cmd);
                            ctx.Response.WriteJson(token);
                        })
                        .Post<SignUp>("sign-up", async (cmd, ctx) =>
                        {
                            await ctx.RequestServices.GetService<IIdentityService>().SignUpAsync(cmd);
                            await ctx.Response.NoContent();
                        })))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
