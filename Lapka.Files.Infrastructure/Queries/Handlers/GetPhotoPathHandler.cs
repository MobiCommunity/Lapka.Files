using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Queries;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Documents;

namespace Lapka.Files.Infrastructure.Queries.Handlers
{
    public class GetPhotoPathHandler : IQueryHandler<GetPhotoPath, PhotoPathDto>
    {
        private readonly IMinioServiceClient _minioServiceClient;
        private readonly IPhotoRepository _repository;

        public GetPhotoPathHandler(IMinioServiceClient minioServiceClient, IPhotoRepository repository)
        {
            _minioServiceClient = minioServiceClient;
            _repository = repository;
        }

        public async Task<PhotoPathDto> HandleAsync(GetPhotoPath query)
        {
            if (!Guid.TryParse(query.Id, out Guid photoId))
            {
                throw new InvalidPhotoIdException(query.Id);
            }
            
            Photo photo = await _repository.GetAsync(photoId);
            if (photo == null)
            {
                throw new PhotoNotFoundException(photoId.ToString());
            }
            
            PhotoPathDto photoDto = new PhotoPathDto
            {
                Path = photo.Path
            };

            return photoDto;
        }
    }
}