using System.Threading.Tasks;
using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Pacco.Services.Identity.Services.Messages.Commands;

namespace Pacco.Services.Identity
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddJwt()
                    .AddCommandHandlers()
                    .AddEventHandlers()
                    .AddQueryHandlers()
                    .AddWebApi())
                .Configure(app => app
                    .UseErrorHandler()
                    .UsePublicMessages()
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync("Welcome to Pacco Identity Service!"))
                        .Post<SignIn>("sign-in", async (req, ctx) =>
                        {
                            var token = await ctx.DispatchAsync<SignIn, string>(req);
                            await ctx.Response.WriteAsync(token);
                        })
                    ))
                .Build()
                .RunAsync();
    }
}
