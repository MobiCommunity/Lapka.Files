using System;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Elastic;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Elastic.Options;
using Lapka.Files.Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Files.Infrastructure.Elastic.Services
{
    public class PhotoElasticsearchUpdater : IPhotoElasticsearchUpdater
    {
        private readonly ILogger<PhotoElasticsearchUpdater> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public PhotoElasticsearchUpdater(ILogger<PhotoElasticsearchUpdater> logger, IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }
        
        public async Task InsertAndUpdateDataAsync(Photo photo)
        {
            IndexResponse response = await _elasticClient.IndexAsync(photo.AsDocument(),
                x => x.Index(_elasticSearchOptions.Aliases.Photos));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to insert or update" +
                                 $" photo {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }

        public async Task DeleteDataAsync(Photo photo)
        {
            DeleteResponse response = await _elasticClient.DeleteAsync<PhotoDocument>(photo.Path,
                x => x.Index(_elasticSearchOptions.Aliases.Photos));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to delete" +
                                 $" photo {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
    }
}