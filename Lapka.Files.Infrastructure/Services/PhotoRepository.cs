using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Documents;

namespace Lapka.Files.Infrastructure.Services
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IMongoRepository<PhotoDocument, Guid> _repository;

        public PhotoRepository(IMongoRepository<PhotoDocument, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<Photo> GetAsync(Guid photoId)
        {
            PhotoDocument photo = await _repository.GetAsync(x => x.Id == photoId);
            return photo?.AsDocument();
        }

        public async Task AddAsync(Guid photoId, string photoPath)
        {
            await _repository.AddAsync(new PhotoDocument(photoId, photoPath));
        }

        public async Task DeleteAsync(Photo photo)
        {
            await _repository.DeleteAsync(photo.AsDocument().Id);
        }
    }
}