using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Queries;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Elastic.Options;
using Lapka.Files.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Files.Infrastructure.Elastic.Queries.Handlers
{
    public class GetPhotoPathHandler : IQueryHandler<GetPhotoPath, PhotoPathDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetPhotoPathHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<PhotoPathDto> HandleAsync(GetPhotoPath query)
        {
            Guid photoId = GetPhotoId(query);
            
            PhotoDocument photo = await GetPhotoDocumentAsync(query, photoId);
            
            return photo.AsDto();
        }

        private async Task<PhotoDocument> GetPhotoDocumentAsync(GetPhotoPath query, Guid photoId)
        {
            GetResponse<PhotoDocument> searchResult =
                await _elasticClient.GetAsync<PhotoDocument>(photoId,
                    x => x.Index(_elasticSearchOptions.Aliases.Photos));

            PhotoDocument photo = searchResult?.Source;
            if (photo is null)
            {
                throw new PhotoNotFoundException(query.Id);
            }

            return photo;
        }

        private static Guid GetPhotoId(GetPhotoPath query)
        {
            if (!Guid.TryParse(query.Id, out Guid photoId))
            {
                throw new InvalidPhotoIdException(query.Id);
            }

            return photoId;
        }
    }
}