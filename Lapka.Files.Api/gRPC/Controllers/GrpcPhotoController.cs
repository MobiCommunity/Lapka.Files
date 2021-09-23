using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Grpc.Core;
using Lapka.Files.Api.Models;
using Lapka.Files.Application.Commands;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Queries;
using Lapka.Files.Application.Services.Photos;

namespace Lapka.Files.Api.gRPC.Controllers
{
    public class GrpcPhotoController : PhotoProto.PhotoProtoBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMinioPhotoCreator _photoCreator;

        public GrpcPhotoController(ICommandDispatcher commandDispatcher, IMinioPhotoCreator photoCreator)
        {
            _commandDispatcher = commandDispatcher;
            _photoCreator = photoCreator;
        }

        public override async Task<UploadPhotoReply> UploadPhoto(UploadPhotoRequest request, ServerCallContext context)
        {
            string photo = await _photoCreator.CreatePhotoToMinioAsync(new UploadPhoto(request.UserId, request.IsPublic,
                request.Name, request.Photo.ToByteArray(), request.BucketName.AsValueObject()));

            return new UploadPhotoReply
            {
                Path = photo
            };
        }

        public override async Task<DeletePhotoReply> DeletePhoto(DeletePhotoRequest request, ServerCallContext context)
        {
            await _commandDispatcher.SendAsync(new DeletePhoto(request.Id, request.UserId,
                request.BucketName.AsValueObject()));

            return new DeletePhotoReply();
        }
    }
}