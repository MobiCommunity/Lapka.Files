using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Grpc.Core;
using Lapka.Files.Api.Models;
using Lapka.Files.Application.Commands;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Queries;

namespace Lapka.Files.Api.gRPC.Controllers
{
    public class GrpcPhotoController : Photo.PhotoBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public GrpcPhotoController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public override async Task<GetPhotoPathReply> GetPhotoPath(GetPhotoPathRequest request, ServerCallContext context)
        {
            PhotoPathDto result = await _queryDispatcher.QueryAsync(new GetPhotoPath
            {
                Id = request.Id,
                BucketName = request.BucketName.AsValueObject()
            });

            return new GetPhotoPathReply
            {
                Path = result.Path
            };
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

        public override async Task<SetExternalPhotoReply> SetExternalPhoto(SetExternalPhotoRequest request,
            ServerCallContext context)
        {
            await _commandDispatcher.SendAsync(new SetExternalPhoto(request.Id, request.OldName, request.NewName,
                request.BucketName.AsValueObject()));

            return new SetExternalPhotoReply();
        }
    }
}