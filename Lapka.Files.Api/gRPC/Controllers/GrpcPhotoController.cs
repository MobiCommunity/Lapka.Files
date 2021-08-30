using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Grpc.Core;
using Lapka.Files.Api.Models;
using Lapka.Files.Application.Commands;

namespace Lapka.Files.Api.gRPC.Controllers
{
    public class GrpcPhotoController : Photo.PhotoBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public GrpcPhotoController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public override async Task<UploadPhotoReply> UploadPhoto(UploadPhotoRequest request, ServerCallContext context)
        {
            await _commandDispatcher.SendAsync(new UploadPhoto(request.Id, request.Name, request.Photo.ToByteArray(),
                request.BucketName.AsValueObject()));

            return new UploadPhotoReply();
        }

        public override async Task<DeletePhotoReply> DeletePhoto(DeletePhotoRequest request, ServerCallContext context)
        {
            await _commandDispatcher.SendAsync(new DeletePhoto(request.Id, request.BucketName.AsValueObject()));

            return new DeletePhotoReply();
        }
    }
}