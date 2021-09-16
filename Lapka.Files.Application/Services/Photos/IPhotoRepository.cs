using System;
using System.Threading.Tasks;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Services.Photos
{
    public interface IPhotoRepository
    {
        public Task<Photo> GetAsync(Guid photoId);
        public Task<Photo> GetAsync(string photoPath);
        public Task AddAsync(Photo photo);
        public Task DeleteAsync(Photo photo);
    }
}