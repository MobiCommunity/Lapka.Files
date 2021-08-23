using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Queries;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Infrastructure.Queries.Handlers
{
    public class GetPhotoHandler : IQueryHandler<GetPhoto, PhotoDto>
    {
        private readonly IMinioServiceClient _minioServiceClient;

        public GetPhotoHandler(IMinioServiceClient minioServiceClient)
        {
            _minioServiceClient = minioServiceClient;
        }

        public async Task<PhotoDto> HandleAsync(GetPhoto query)
        {
            PhotoDto photoDto = new PhotoDto
            {
                Content = await _minioServiceClient.GetAsync(query.Path, query.BucketName)
            };

            return photoDto;
        }
    }
}