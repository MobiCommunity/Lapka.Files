using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Lapka.Files.Application.Services;

namespace Lapka.Files.Infrastructure.Grpc.Services
{
    public class GrpcPetPhotoService : IGrpcPetPhotoService
    {
        private readonly PetPhotosProto.PetPhotosProtoClient _client;

        public GrpcPetPhotoService(PetPhotosProto.PetPhotosProtoClient client)
        {
            _client = client;
        }
        
        public async Task<IEnumerable<string>> GetShelterPetPhotosAsync(Guid shelterId)
        {
            GetShelterPetPhotosReply response = await _client.GetShelterPetPhotosAsync(new GetShelterPetPhotosRequest
            {
                PetId = shelterId.ToString()
            });

            Collection<string> paths = new Collection<string>();

            foreach (string path in response.Paths)
            {
                paths.Add(path);
            }

            return paths;
        }

        public async Task<IEnumerable<string>> GetLostPetPhotosAsync(Guid userId)
        {
            GetLostPetPhotosReply response = await _client.GetLostPetPhotosAsync(new GetLostPetPhotosRequest
            {
                PetId = userId.ToString()
            });

            Collection<string> paths = new Collection<string>();

            foreach (string path in response.Paths)
            {
                paths.Add(path);
            }

            return paths;
        }

        public async Task<IEnumerable<string>> GetUserPetPhotosAsync(Guid petId)
        {
            GetUserPetPhotosReply response = await _client.GetUserPetPhotosAsync(new GetUserPetPhotosRequest
            {
                PetId = petId.ToString()
            });

            Collection<string> paths = new Collection<string>();

            foreach (string path in response.Paths)
            {
                paths.Add(path);
            }

            return paths;
        }
    }
}