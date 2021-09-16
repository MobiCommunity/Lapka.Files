using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Files.Infrastructure.Elastic.Options;
using Lapka.Files.Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Files.Infrastructure.Elastic.Services
{
    public class ElasticSearchSeeder : IHostedService
    {
        private readonly ILogger<ElasticSearchSeeder> _logger;
        private readonly IMongoRepository<PhotoDocument, Guid> _photoRepository;
        private readonly IElasticClient _client;
        private readonly ElasticSearchOptions _elasticOptions;

        public ElasticSearchSeeder(ILogger<ElasticSearchSeeder> logger,
            IMongoRepository<PhotoDocument, Guid> photoRepository, IElasticClient client,
            ElasticSearchOptions elasticOptions)
        {
            _logger = logger;
            _photoRepository = photoRepository;
            _client = client;
            _elasticOptions = elasticOptions;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SeedPhotosAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        private async Task SeedPhotosAsync()
        {
            IReadOnlyList<PhotoDocument> photos = await _photoRepository.FindAsync(_ => true);

            BulkAllObservable<PhotoDocument> bulkPhotos =
                _client.BulkAll(photos, b => b.Index(_elasticOptions.Aliases.Photos));

            bulkPhotos.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Photos indexed"));
        }
    }
}