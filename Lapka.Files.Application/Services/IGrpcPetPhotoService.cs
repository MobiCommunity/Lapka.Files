using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lapka.Files.Application.Services
{
    public interface IGrpcPetPhotoService
    {
        Task<IEnumerable<string>> GetShelterPetPhotosAsync(Guid shelterId);
        Task<IEnumerable<string>> GetLostPetPhotosAsync(Guid userId);
        Task<IEnumerable<string>> GetUserPetPhotosAsync(Guid petId);
    }
}