using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands.Handlers
{
    public class DeletePhotoHandler : ICommandHandler<DeletePhoto>
    {
        private readonly IMinioServiceClient _minioServiceClient;
        private readonly IPhotoRepository _photoRepository;

        public DeletePhotoHandler(IMinioServiceClient minioServiceClient, IPhotoRepository photoRepository)
        {
            _minioServiceClient = minioServiceClient;
            _photoRepository = photoRepository;
        }
        
        public async Task HandleAsync(DeletePhoto command)
        {
            if (!Guid.TryParse(command.Id, out Guid photoId))
            {
                throw new InvalidPhotoIdException(command.Id);
            }

            Photo photo = await _photoRepository.GetAsync(photoId);
            if (photo == null)
            {
                throw new PhotoNotFoundException(command.Id);
            }
            try
            {
                await _minioServiceClient.DeleteAsync(photo.Path, command.BucketName);
            }
            catch (Exception ex)
            {
                throw new CannotConnetToMinioException(ex, "Error at deleting a photo");
            }
            
            photo.Delete();
            await _photoRepository.DeleteAsync(photo);
        }
    }
}