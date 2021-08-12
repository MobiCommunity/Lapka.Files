using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Google.Protobuf;
using Grpc.Core;
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
            await _commandDispatcher.SendAsync(new UploadPhoto(request.PhotoPath, request.Photo.ToByteArray()));

            return new UploadPhotoReply();
        }

        public override async Task<DeletePhotoReply> DeletePhoto(DeletePhotoRequest request, ServerCallContext context)
        {
            await _commandDispatcher.SendAsync(new DeletePhoto(request.PhotoPath));

            return new DeletePhotoReply();
        }
    }
}