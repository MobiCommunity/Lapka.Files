using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Lapka.Files.Application.Services;

namespace Lapka.Files.Infrastructure.Grpc.Services
{
    public class GrpcIdentityPhotoService : IGrpcIdentityPhotoService
    {
        private readonly IdentityPhotoProto.IdentityPhotoProtoClient _client;

        public GrpcIdentityPhotoService(IdentityPhotoProto.IdentityPhotoProtoClient client)
        {
            _client = client;
        }
        
        public async Task<string> GetShelterPhotoAsync(Guid shelterId)
        {
            GetShelterPhotoReply response = await _client.GetShelterPhotoAsync(new GetShelterPhotoRequest
            {
                ShelterId = shelterId.ToString()
            });
            
            return response.Path;
        }

        public async Task<string> GetUserPhotoAsync(Guid userId)
        {
            GetUserPhotoReply response = await _client.GetUserPhotoAsync(new GetUserPhotoRequest
            {
                UserId = userId.ToString()
            });
            
            return response.Path;
        }
    }
}