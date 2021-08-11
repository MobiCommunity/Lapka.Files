using System.IO;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands.Handlers
{
    public class UploadPhotoHandler : ICommandHandler<UploadPhoto>
    {
        private readonly IMinioServiceClient _minioServiceClient;

        public UploadPhotoHandler(IMinioServiceClient minioServiceClient)
        {
            _minioServiceClient = minioServiceClient;
        }
        
        public async Task HandleAsync(UploadPhoto command)
        {
            await using MemoryStream photoStream = new MemoryStream(command.Photo);

            Photo photo = new Photo(command.Path, photoStream);
            
            await _minioServiceClient.AddAsync(photo);
        }
    }
}