using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Minios;
using Lapka.Files.Application.Services.Photos;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Events.External.Handlers
{
    public class ShelterRemovedHandler : IEventHandler<ShelterRemoved>
    {
        private readonly IMinioServiceClient _minioServiceClient;
        private readonly IPhotoRepository _photoRepository;
        private readonly IGrpcIdentityPhotoService _grpcIdentityPhotoService;

        public ShelterRemovedHandler(IMinioServiceClient minioServiceClient, IPhotoRepository photoRepository,
            IGrpcIdentityPhotoService grpcIdentityPhotoService)
        {
            _minioServiceClient = minioServiceClient;
            _photoRepository = photoRepository;
            _grpcIdentityPhotoService = grpcIdentityPhotoService;
        }

        public async Task HandleAsync(ShelterRemoved @event)
        {
            string path = await _grpcIdentityPhotoService.GetShelterPhotoAsync(@event.ShelterId);
            
            Photo photo = await GetPhotoAsync(path);
        
            await DeletePhotoFromMinioAsync(path);
        
            await _photoRepository.DeleteAsync(photo);
        }

        private async Task<Photo> GetPhotoAsync(string path)
        {
            Photo photo = await _photoRepository.GetAsync(path);
            if (photo is null)
            {
                throw new PhotoNotFoundException(path);
            }

            return photo;
        }

        private async Task DeletePhotoFromMinioAsync(string path)
        {
            try
            {
                await _minioServiceClient.DeleteAsync(path, BucketName.ShelterPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotConnectToMinioException(ex, "Error at deleting a photo");
            }
        }
    }
}