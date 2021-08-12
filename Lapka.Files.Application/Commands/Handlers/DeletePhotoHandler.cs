using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.Entities;

namespace Lapka.Files.Application.Commands.Handlers
{
    public class DeletePhotoHandler : ICommandHandler<DeletePhoto>
    {
        private readonly IMinioServiceClient _minioServiceClient;

        public DeletePhotoHandler(IMinioServiceClient minioServiceClient)
        {
            _minioServiceClient = minioServiceClient;
        }
        
        public async Task HandleAsync(DeletePhoto command)
        {
            await _minioServiceClient.DeleteAsync(command.Path);
        }
    }
}