using System;
using System.Threading.Tasks;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Services.Photos
{
    public interface IPhotoRepository
    {
        public Task<Photo> GetAsync(string photoId);
        public Task AddAsync(Photo photo);
        public Task DeleteAsync(Photo photo);
    }
}