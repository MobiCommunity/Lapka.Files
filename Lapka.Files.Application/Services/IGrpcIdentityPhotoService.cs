using System;
using System.Threading.Tasks;

namespace Lapka.Files.Application.Services
{
    public interface IGrpcIdentityPhotoService
    {
        Task<string> GetUserPhotoAsync(Guid userId);
        Task<string> GetShelterPhotoAsync(Guid shelterId);

    }
}