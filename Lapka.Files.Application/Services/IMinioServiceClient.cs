using System.Threading.Tasks;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Services
{
    public interface IMinioServiceClient
    {
        Task AddAsync(Photo photo);
        Task DeleteAsync(string photoPath);
        Task<byte[]> GetAsync(string path);
    }
}