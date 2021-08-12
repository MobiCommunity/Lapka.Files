using Convey;
using Convey.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Open.Serialization.Json.Newtonsoft;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Lapka.Files.Api.Attributes;
using Lapka.Files.Api.gRPC.Controllers;
using Lapka.Files.Application;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure;
using Lapka.Files.Infrastructure.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Lapka.Files.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateWebHostBuilder(args).Build().RunAsync();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(5003, o => o.Protocols = HttpProtocols.Http1);
                    options.ListenAnyIP(5013, o => o.Protocols = HttpProtocols.Http2);
                }).ConfigureServices(services =>
                {
                    services.AddControllers();

                    services.TryAddSingleton(new JsonSerializerFactory().GetSerializer());
                    
                    services.AddScoped<IMinioServiceClient, MinioServiceClient>();

                    services
                        .AddConvey()
                        .AddInfrastructure()
                        .AddApplication();

                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                    services.AddGrpc();

                    services.AddSwaggerGen(c =>
                    {

                        c.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Version = "v1",
                            Title = "Files Microservice",
                            Description = ""
                        });
                        string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        string xmlFile2 = "Lapka.Files.Application.xml";
                        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                        string xmlPath2 = Path.Combine(AppContext.BaseDirectory, xmlFile2);
                        c.OperationFilter<BasicAuthOperationsFilter>();
                        c.IncludeXmlComments(xmlPath);
                        c.IncludeXmlComments(xmlPath2);
                    });

                    services.BuildServiceProvider();
                }).Configure(app =>
                {
                    app
                        .UseConvey()
                        .UseInfrastructure()
                        .UseRouting()
                        .UseSwagger(c => { c.RouteTemplate = "api/files/swagger/{documentname}/swagger.json"; })
                        .UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/api/files/swagger/v1/swagger.json", "My API V1");
                            c.RoutePrefix = "api/files/swagger";
                        })
                        .UseEndpoints(e =>
                        {
                            e.MapGrpcService<GrpcPhotoController>();
                            e.MapControllers();
                            e.Map("ping", async ctx => { await ctx.Response.WriteAsync("Alive"); });
                        });
                })
                .UseLogging();
    }
}