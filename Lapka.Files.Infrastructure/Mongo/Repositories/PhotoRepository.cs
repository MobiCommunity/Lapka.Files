using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Photos;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Mongo.Documents;

namespace Lapka.Files.Infrastructure.Mongo.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IMongoRepository<PhotoDocument, Guid> _repository;

        public PhotoRepository(IMongoRepository<PhotoDocument, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<Photo> GetAsync(string photoId)
        {
            PhotoDocument photo = await _repository.GetAsync(x => x.Path == photoId);
            return photo?.AsBusiness();
        }

        public async Task AddAsync(Photo photo)
        {
            await _repository.AddAsync(photo.AsDocument());
        }

        public async Task DeleteAsync(Photo photo)
        {
            await _repository.DeleteAsync(photo.AsDocument().Id);
        }
    }
}