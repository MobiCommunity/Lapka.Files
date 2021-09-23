using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Queries;
using Lapka.Files.Application.Services.Minios;
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
            if (photo.UserId != query.UserId && !photo.IsPublic)
            {
                throw new PhotoIsPrivateException(query.PhotoPath);
            }

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
                    Content = await _minioServiceClient.GetAsync(photo.Path, query.BucketName)
                };
            }
            catch (Minio.Exceptions.ObjectNotFoundException)
            {
                throw new PhotoNotFoundException(query.PhotoPath);
            }

            return photoDto;
        }

        private async Task<PhotoDocument> GetPhotoDocumentAsync(GetPhoto query)
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.Photos)
            {
                Query = new MatchQuery
                {
                    Query = query.PhotoPath,
                    Field = Infer.Field<PhotoDocument>(p => p.Path)
                }
            };
            
            ISearchResponse<PhotoDocument> searchResult = await _elasticClient.SearchAsync<PhotoDocument>(searchRequest);
            
            PhotoDocument photo = searchResult?.Documents.FirstOrDefault();
            if (photo is null)
            {
                throw new PhotoNotFoundException(query.PhotoPath);
            }

            return photo;
        }
    }
}