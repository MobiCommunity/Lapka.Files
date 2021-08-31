using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands.Handlers
{
    public class SetExternalPhotoHandler : ICommandHandler<SetExternalPhoto>
    {
        private readonly IMinioServiceClient _minioServiceClient;
        private readonly IPhotoRepository _photoRepository;

        public SetExternalPhotoHandler(IMinioServiceClient minioServiceClient, IPhotoRepository photoRepository)
        {
            _minioServiceClient = minioServiceClient;
            _photoRepository = photoRepository;
        }
        
        public async Task HandleAsync(SetExternalPhoto command)
        {
            if (!Guid.TryParse(command.Id, out Guid photoId))
            {
                throw new InvalidPhotoIdException(command.Id);
            }

            if (!string.IsNullOrEmpty(command.OldName))
            {
                await DeletePhotoAsync(command);
            }

            await using MemoryStream photoStream = new MemoryStream(await GetPhotoFromExternalSourceAsync(command));
            
            Photo photo = Photo.Create(photoId, command.NewName, photoStream);
            
            await AddPhotoToMinioAsync(command, photo);
            await _photoRepository.AddAsync(photoId, command.NewName);
        }
        
        private async Task AddPhotoToMinioAsync(SetExternalPhoto command, Photo photo)
        {
            try
            {
                await _minioServiceClient.AddAsync(photo, command.BucketName);
            }
            catch (Exception ex)
            {
                throw new CannotConnetToMinioException(ex, "Error at adding a photo");
            }
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