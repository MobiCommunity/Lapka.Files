using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Services.Minios;
using Lapka.Files.Application.Services.Photos;
using Lapka.Files.Core.Entities;

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
            Photo photo = await GetPhotoAsync(command);
            
            await DeletePhotoFromMinioAsync(command, photo);

            await _photoRepository.DeleteAsync(photo);
        }

        private async Task<Photo> GetPhotoAsync(DeletePhoto command)
        {
            Photo photo = await _photoRepository.GetAsync(command.PhotoPath);
            if (photo == null)
            {
                throw new PhotoNotFoundException(command.PhotoPath);
            }

            return photo;
        }

        private async Task DeletePhotoFromMinioAsync(DeletePhoto command, Photo photo)
        {
            try
            {
                await _minioServiceClient.DeleteAsync(photo.Path, command.BucketName);
            }
            catch (Exception ex)
            {
                throw new CannotConnectToMinioException(ex, "Error at deleting a photo");
            }
        }
    }
}