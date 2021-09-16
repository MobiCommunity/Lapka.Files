using System.Threading.Tasks;
using Lapka.Files.Application.Events.Abstract;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Elastic;
using Lapka.Files.Core.Events.Concrete;

namespace Lapka.Files.Application.Events.Internal.Handler
{
    public class PhotoDeletedEventHandler : IDomainEventHandler<PhotoDeleted>
    {
        private readonly IPhotoElasticsearchUpdater _elasticsearchUpdater;

        public PhotoDeletedEventHandler(IPhotoElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }

        public async Task HandleAsync(PhotoDeleted @event)
        {
            await _elasticsearchUpdater.DeleteDataAsync(@event.Photo);
        }
    }
}