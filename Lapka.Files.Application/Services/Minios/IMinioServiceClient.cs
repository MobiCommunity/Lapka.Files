using System.Threading.Tasks;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Services.Minios
{
    public interface IMinioServiceClient
    {
        Task AddAsync(Photo photo, BucketName bucket);
        Task DeleteAsync(string path, BucketName bucket);
        Task<byte[]> GetAsync(string path, BucketName bucket);
    }
}