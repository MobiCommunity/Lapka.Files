using System.Threading.Tasks;
using Lapka.Files.Application.Commands;

namespace Lapka.Files.Application.Services.Photos
{
    public interface IMinioPhotoCreator
    {
        public Task<string> CreatePhotoToMinioAsync(UploadPhoto photo);
    }
}