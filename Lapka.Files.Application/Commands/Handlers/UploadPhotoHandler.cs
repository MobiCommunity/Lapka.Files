using System;
using System.IO;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands.Handlers
{
    public class UploadPhotoHandler : ICommandHandler<UploadPhoto>
    {
        private readonly IMinioServiceClient _minioServiceClient;
        private readonly IPhotoRepository _photoRepository;

        public UploadPhotoHandler(IMinioServiceClient minioServiceClient, IPhotoRepository photoRepository)
        {
            _minioServiceClient = minioServiceClient;
            _photoRepository = photoRepository;
        }
        
        public async Task HandleAsync(UploadPhoto command)
        {
            if (!Guid.TryParse(command.Id, out Guid photoId))
            {
                throw new InvalidPhotoIdException(command.Id);
            }
            string photoPath = $"{photoId}.{command.GetFileExtension()}";
            await using MemoryStream photoStream = new MemoryStream(command.Photo);

            Photo photo = Photo.Create(photoId, photoPath, photoStream);
            try
            {
                await _minioServiceClient.AddAsync(photo, command.BucketName);
            }
            catch (Exception ex)
            {
                throw new CannotConnetToMinioException(ex, "Error at adding a photo");
            }
            
            await _photoRepository.AddAsync(photoId, photoPath);
        }
    }
}