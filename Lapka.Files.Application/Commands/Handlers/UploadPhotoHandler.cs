using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Files.Application.Services.Photos;

namespace Lapka.Files.Application.Commands.Handlers
{
    public class UploadPhotoHandler : ICommandHandler<UploadPhoto>
    {
        private readonly IMinioPhotoCreator _photoCreator;

        public UploadPhotoHandler(IMinioPhotoCreator photoCreator)
        {
            _photoCreator = photoCreator;
        }

        public async Task HandleAsync(UploadPhoto command) => await _photoCreator.CreatePhotoToMinioAsync(command);
    }
}