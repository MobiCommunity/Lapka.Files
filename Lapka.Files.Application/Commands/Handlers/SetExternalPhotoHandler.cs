using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Minios;
using Lapka.Files.Application.Services.Photos;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands.Handlers
{
    public class SetExternalPhotoHandler : ICommandHandler<SetExternalPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IMinioServiceClient _minioServiceClient;
        private readonly IPhotoRepository _photoRepository;

        public SetExternalPhotoHandler(IEventProcessor eventProcessor, IMinioServiceClient minioServiceClient,
            IPhotoRepository photoRepository)
        {
            _eventProcessor = eventProcessor;
            _minioServiceClient = minioServiceClient;
            _photoRepository = photoRepository;
        }

        public async Task HandleAsync(SetExternalPhoto command)
        {
            Guid photoId = GetAndParsePhotoId(command);
            await ValidIfThisIsNewPhotoAsync(command);

            Photo photo = await CreateAndAddPhotoToMinio(command, photoId);

            await _photoRepository.AddAsync(photo);
            await _eventProcessor.ProcessAsync(photo.Events);
        }

        private async Task<Photo> CreateAndAddPhotoToMinio(SetExternalPhoto command, Guid photoId)
        {
            await using MemoryStream photoStream = new MemoryStream(await GetPhotoFromExternalSourceAsync(command));
            Photo photo = Photo.Create(photoId, command.NewName, photoStream);

            try
            {
                await _minioServiceClient.AddAsync(photo, command.BucketName);
            }
            catch (Exception ex)
            {
                throw new CannotConnetToMinioException(ex, "Error at adding a photo");
            }

            return photo;
        }

        private async Task ValidIfThisIsNewPhotoAsync(SetExternalPhoto command)
        {
            if (!string.IsNullOrEmpty(command.OldName))
            {
                await DeletePhotoAsync(command);
            }
        }

        private static Guid GetAndParsePhotoId(SetExternalPhoto command)
        {
            if (!Guid.TryParse(command.Id, out Guid photoId))
            {
                throw new InvalidPhotoIdException(command.Id);
            }

            return photoId;
        }

        private static async Task<byte[]> GetPhotoFromExternalSourceAsync(SetExternalPhoto command)
        {
            byte[] response = await new WebClient().DownloadDataTaskAsync(new Uri(command.NewName));
            return response;
        }

        private async Task DeletePhotoAsync(SetExternalPhoto command)
        {
            Photo photoToDelete = await _photoRepository.GetAsync(command.OldName);
            try
            {
                await _minioServiceClient.DeleteAsync(command.OldName, command.BucketName);
            }
            catch (Exception ex)
            {
                throw new CannotConnetToMinioException(ex, "Error at adding a photo");
            }

            photoToDelete.Delete();
            await _photoRepository.DeleteAsync(photoToDelete);
        }
    }
}