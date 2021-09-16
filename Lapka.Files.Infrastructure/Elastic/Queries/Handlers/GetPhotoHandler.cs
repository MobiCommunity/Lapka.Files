using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Queries;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Minios;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Elastic.Options;
using Lapka.Files.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Files.Infrastructure.Elastic.Queries.Handlers
{
    public class GetPhotoHandler : IQueryHandler<GetPhoto, PhotoDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;
        private readonly IMinioServiceClient _minioServiceClient;

        public GetPhotoHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions,
            IMinioServiceClient minioServiceClient)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
            _minioServiceClient = minioServiceClient;
        }
        
        public async Task<PhotoDto> HandleAsync(GetPhoto query)
        {
            PhotoDocument photo = await GetPhotoDocumentAsync(query);

            PhotoDto photoDto = await GetPhotoFromMinioAndConvertToDto(query, photo);

            return photoDto;
        }

        private async Task<PhotoDto> GetPhotoFromMinioAndConvertToDto(GetPhoto query, PhotoDocument photo)
        {
            PhotoDto photoDto;
            try
            {
                photoDto = new PhotoDto
                {
                    Content = await _minioServiceClient.GetAsync(photo.PhotoPath, query.BucketName)
                };
            }
            catch (Minio.Exceptions.ObjectNotFoundException)
            {
                throw new PhotoNotFoundException(query.Id.ToString());
            }

            return photoDto;
        }

        private async Task<PhotoDocument> GetPhotoDocumentAsync(GetPhoto query)
        {
            GetResponse<PhotoDocument> searchResult =
                await _elasticClient.GetAsync<PhotoDocument>(query.Id,
                    x => x.Index(_elasticSearchOptions.Aliases.Photos));
            
            PhotoDocument photo = searchResult?.Source;
            if (photo == null)
            {
                throw new PhotoNotFoundException(query.Id.ToString());
            }

            return photo;
        }
    }
}