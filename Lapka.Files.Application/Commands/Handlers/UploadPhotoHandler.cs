using System;
using System.IO;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Minios;
using Lapka.Files.Application.Services.Photos;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands.Handlers
{
    public class UploadPhotoHandler : ICommandHandler<UploadPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IMinioServiceClient _minioServiceClient;
        private readonly IPhotoRepository _photoRepository;

        public UploadPhotoHandler(IEventProcessor eventProcessor, IMinioServiceClient minioServiceClient,
            IPhotoRepository photoRepository)
        {
            _eventProcessor = eventProcessor;
            _minioServiceClient = minioServiceClient;
            _photoRepository = photoRepository;
        }

        public async Task HandleAsync(UploadPhoto command)
        {
            Guid photoId = GetAndParsePhotoId(command);

            Photo photo = await CreatePhotoAsync(command, photoId);
            
            await _photoRepository.AddAsync(photo);
            await _eventProcessor.ProcessAsync(photo.Events);
        }

        private async Task<Photo> CreatePhotoAsync(UploadPhoto command, Guid photoId)
        {
            string photoPath = $"{photoId}.{command.GetFileExtension()}";
            await using MemoryStream photoStream = new MemoryStream(command.Photo);

            Photo photo = Photo.Create(photoId, photoPath, photoStream);
            
            try
            {
                await _minioServiceClient.AddAsync(photo, command.BucketName);
            }
            catch (Exception ex)
            {
                throw new CannotConnectToMinioException(ex, "Error at adding a photo");
            }
            
            return photo;
        }
        
        private static Guid GetAndParsePhotoId(UploadPhoto command)
        {
            if (!Guid.TryParse(command.Id, out Guid photoId))
            {
                throw new InvalidPhotoIdException(command.Id);
            }

            return photoId;
        }
    }
}