using Convey;
using Convey.CQRS.Queries;
using Convey.HTTP;
using Convey.MessageBrokers.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Convey.Auth;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.Persistence.MongoDB;
using Lapka.Files.Application.Events.Abstract;
using Lapka.Files.Application.Events.External;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Elastic;
using Lapka.Files.Application.Services.Photos;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Elastic.Options;
using Lapka.Files.Infrastructure.Elastic.Services;
using Lapka.Files.Infrastructure.Exceptions;
using Lapka.Files.Infrastructure.Grpc.Options;
using Lapka.Files.Infrastructure.Grpc.Services;
using Lapka.Files.Infrastructure.Minios.Options;
using Lapka.Files.Infrastructure.Mongo.Documents;
using Lapka.Files.Infrastructure.Mongo.Repositories;
using Lapka.Files.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Nest;


namespace Lapka.Files.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHttpClient()
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddMongo()
                .AddMongoRepository<PhotoDocument, Guid>("photos")
                .AddJwt()
                .AddRabbitMq()
                .AddMessageOutbox()
                // .AddConsul()
                // .AddFabio()
                // .AddMetrics()
                ;

            builder.Services.Configure<KestrelServerOptions>
                (o => o.AllowSynchronousIO = true);

            builder.Services.Configure<IISServerOptions>(o => o.AllowSynchronousIO = true);

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            IServiceCollection services = builder.Services;

            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IConfiguration>();

            MinioOptions minioOptions = new MinioOptions();
            configuration.GetSection("minio").Bind(minioOptions);
            services.AddSingleton(minioOptions);
            
            IdentityMicroserviceOptions identityMicroserviceOptions = new IdentityMicroserviceOptions();
            configuration.GetSection("identityMicroservice").Bind(identityMicroserviceOptions);
            services.AddSingleton(identityMicroserviceOptions);
            
            PetsMicroserviceOptions petsMicroserviceOptions = new PetsMicroserviceOptions();
            configuration.GetSection("petsMicroservice").Bind(petsMicroserviceOptions);
            services.AddSingleton(petsMicroserviceOptions);
            
            services.AddGrpcClient<PetPhotosProto.PetPhotosProtoClient>(o =>
            {
                o.Address = new Uri(petsMicroserviceOptions.UrlHttp2);
            });
            
            services.AddGrpcClient<IdentityPhotoProto.IdentityPhotoProtoClient>(o =>
            {
                o.Address = new Uri(identityMicroserviceOptions.UrlHttp2);
            });
            
            ElasticSearchOptions elasticSearchOptions = new ElasticSearchOptions();
            configuration.GetSection("elasticSearch").Bind(elasticSearchOptions);
            services.AddSingleton(elasticSearchOptions);
            ConnectionSettings elasticConnectionSettings = new ConnectionSettings(new Uri(elasticSearchOptions.Url));
            
            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddSingleton<IDomainToIntegrationEventMapper, DomainToIntegrationEventMapper>();
            services.AddSingleton<IPhotoRepository, PhotoRepository>();
            services.AddSingleton<IElasticClient>(new ElasticClient(elasticConnectionSettings));

            services.AddTransient<IMinioPhotoCreator, MinioPhotoCreator>();
            services.AddTransient<IGrpcPetPhotoService, GrpcPetPhotoService>();
            services.AddTransient<IGrpcIdentityPhotoService, GrpcIdentityPhotoService>();
            services.AddTransient<IPhotoElasticsearchUpdater, PhotoElasticsearchUpdater>();
            services.AddTransient<IEventProcessor, EventProcessor>();
            services.AddTransient<IMessageBroker, MessageBroker>();
            
            services.AddHostedService<ElasticSearchSeeder>();

            builder.Services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces().WithTransientLifetime());

            builder.Build();

            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app
                .UseErrorHandler()
                .UseConvey()
                .UseAuthentication()
                .UseRabbitMq()
                .SubscribeEvent<LostPetPhotosRemoved>()
                .SubscribeEvent<LostPetRemoved>()
                .SubscribeEvent<ShelterPetPhotosRemoved>()
                .SubscribeEvent<ShelterPetRemoved>()
                .SubscribeEvent<ShelterRemoved>()
                .SubscribeEvent<UserPetPhotosRemoved>()
                .SubscribeEvent<UserPetRemoved>()
                .SubscribeEvent<UserRemoved>()
                //.UseMetrics()
                ;
            
            return app;
        }
        
        public static async Task<Guid> AuthenticateUsingJwtGetUserIdAsync(this HttpContext context)
        {
            AuthenticateResult authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            return authentication.Succeeded ? Guid.Parse(authentication.Principal.Identity.Name) : Guid.Empty;
        }


    }
}