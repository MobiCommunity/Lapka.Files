using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Minios;
using Lapka.Files.Application.Services.Photos;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands.Handlers
{
    public class DeletePhotoHandler : ICommandHandler<DeletePhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IMinioServiceClient _minioServiceClient;
        private readonly IPhotoRepository _photoRepository;

        public DeletePhotoHandler(IEventProcessor eventProcessor, IMinioServiceClient minioServiceClient,
            IPhotoRepository photoRepository)
        {
            _eventProcessor = eventProcessor;
            _minioServiceClient = minioServiceClient;
            _photoRepository = photoRepository;
        }

        public async Task HandleAsync(DeletePhoto command)
        {
            Guid photoId = GetAndParsePhotoId(command);

            Photo photo = await GetPhotoFromRepositoryAsync(command, photoId);
            await DeletePhotoFromMinioAsync(command, photo);

            photo.Delete();
            await _photoRepository.DeleteAsync(photo);
            await _eventProcessor.ProcessAsync(photo.Events);
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

        private async Task<Photo> GetPhotoFromRepositoryAsync(DeletePhoto command, Guid photoId)
        {
            Photo photo = await _photoRepository.GetAsync(photoId);
            if (photo == null)
            {
                throw new PhotoNotFoundException(command.Id);
            }

            return photo;
        }

        private static Guid GetAndParsePhotoId(DeletePhoto command)
        {
            if (!Guid.TryParse(command.Id, out Guid photoId))
            {
                throw new InvalidPhotoIdException(command.Id);
            }

            return photoId;
        }
    }
}