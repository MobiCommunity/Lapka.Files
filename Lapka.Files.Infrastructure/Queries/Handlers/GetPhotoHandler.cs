using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Queries;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Documents;

namespace Lapka.Files.Infrastructure.Queries.Handlers
{
    public class GetPhotoHandler : IQueryHandler<GetPhoto, PhotoDto>
    {
        private readonly IMinioServiceClient _minioServiceClient;
        private readonly IPhotoRepository _repository;

        public GetPhotoHandler(IMinioServiceClient minioServiceClient, IPhotoRepository repository)
        {
            _minioServiceClient = minioServiceClient;
            _repository = repository;
        }

        public async Task<PhotoDto> HandleAsync(GetPhoto query)
        {
            Photo photo = await _repository.GetAsync(query.Id);
            
            PhotoDto photoDto = new PhotoDto
            {
                Content = await _minioServiceClient.GetAsync(photo.Path, query.BucketName)
            };

            return photoDto;
        }
    }
}