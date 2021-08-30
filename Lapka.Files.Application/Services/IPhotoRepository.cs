using System;
using System.Threading.Tasks;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Services
{
    public interface IPhotoRepository
    {
        public Task<Photo> GetAsync(Guid photoId);
        public Task AddAsync(Guid photoId, string photoPath);
        public Task DeleteAsync(Photo photo);
    }
}