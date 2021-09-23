using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Lapka.Files.Application.Commands;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Minios;
using Lapka.Files.Application.Services.Photos;
using Lapka.Files.Core.Entities;

namespace Lapka.Files.Infrastructure.Services
{
    public class MinioPhotoCreator : IMinioPhotoCreator
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPhotoRepository _photoRepository;
        private readonly IMinioServiceClient _minioServiceClient;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _mapper;

        public MinioPhotoCreator(IEventProcessor eventProcessor, IPhotoRepository photoRepository,
            IMinioServiceClient minioServiceClient, IMessageBroker messageBroker,
            IDomainToIntegrationEventMapper mapper)
        {
            _eventProcessor = eventProcessor;
            _photoRepository = photoRepository;
            _minioServiceClient = minioServiceClient;
            _messageBroker = messageBroker;
            _mapper = mapper;
        }

        public async Task<string> CreatePhotoToMinioAsync(UploadPhoto command)
        {
            if (!Guid.TryParse(command.UserId, out Guid userId))
            {
                throw new InvalidUserIdException(command.UserId);
            }

            string photoPath = $"{Guid.NewGuid():N}.{command.GetFileExtension()}";
            await using MemoryStream photoStream = new MemoryStream(command.Photo);

            Photo photo = Photo.Create(Guid.NewGuid(), photoPath, command.IsPublic, userId, photoStream);

            try
            {
                await _minioServiceClient.AddAsync(photo, command.BucketName);
            }
            catch (Exception ex)
            {
                throw new CannotConnectToMinioException(ex, "Error at adding a photo");
            }

            await _eventProcessor.ProcessAsync(photo.Events);
            await _photoRepository.AddAsync(photo);
            IEnumerable<IEvent> events = _mapper.MapAll(photo.Events);
            await _messageBroker.PublishAsync(events);

            return photoPath;
        }
    }
}