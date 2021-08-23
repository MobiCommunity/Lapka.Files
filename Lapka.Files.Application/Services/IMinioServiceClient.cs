using System.Threading.Tasks;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Services
{
    public interface IMinioServiceClient
    {
        Task AddAsync(Photo photo, BucketName bucket);
        Task DeleteAsync(string photoPath, BucketName bucket);
        Task<byte[]> GetAsync(string path, BucketName bucket);
    }
}