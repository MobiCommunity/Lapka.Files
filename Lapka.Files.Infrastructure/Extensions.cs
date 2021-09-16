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
using Convey.Persistence.MongoDB;
using Lapka.Files.Application.Events.Abstract;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Elastic;
using Lapka.Files.Application.Services.Photos;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Elastic.Options;
using Lapka.Files.Infrastructure.Elastic.Services;
using Lapka.Files.Infrastructure.Exceptions;
using Lapka.Files.Infrastructure.Minios.Options;
using Lapka.Files.Infrastructure.Mongo.Documents;
using Lapka.Files.Infrastructure.Mongo.Repositories;
using Lapka.Files.Infrastructure.Services;
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
                // .AddRabbitMq()
                // .AddConsul()
                // .AddFabio()
                // .AddMessageOutbox()
                // .AddMetrics()
                ;

            builder.Services.Configure<KestrelServerOptions>
                (o => o.AllowSynchronousIO = true);

            builder.Services.Configure<IISServerOptions>(o => o.AllowSynchronousIO = true);

            IServiceCollection services = builder.Services;

            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IConfiguration>();

            MinioOptions minioOptions = new MinioOptions();
            configuration.GetSection("minio").Bind(minioOptions);
            services.AddSingleton(minioOptions);
            
            ElasticSearchOptions elasticSearchOptions = new ElasticSearchOptions();
            configuration.GetSection("elasticSearch").Bind(elasticSearchOptions);
            services.AddSingleton(elasticSearchOptions);
            ConnectionSettings elasticConnectionSettings = new ConnectionSettings(new Uri(elasticSearchOptions.Url));
            
            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddSingleton<IDomainToIntegrationEventMapper, DomainToIntegrationEventMapper>();
            services.AddSingleton<IPhotoRepository, PhotoRepository>();
            services.AddSingleton<IElasticClient>(new ElasticClient(elasticConnectionSettings));

            services.AddTransient<IPhotoElasticsearchUpdater, PhotoElasticsearchUpdater>();
            services.AddTransient<IEventProcessor, EventProcessor>();
            services.AddTransient<IMessageBroker, DummyMessageBroker>();
            
            services.AddHostedService<ElasticSearchSeeder>();

            builder.Services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces().WithTransientLifetime());

            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app
                .UseErrorHandler()
                .UseConvey()
                //.UseMetrics()
                //.UseRabbitMq()
                ;


            return app;
        }


    }
}