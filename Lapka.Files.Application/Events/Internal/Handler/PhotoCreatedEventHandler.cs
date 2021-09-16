using System.Threading.Tasks;
using Lapka.Files.Application.Events.Abstract;
using Lapka.Files.Application.Services;
using Lapka.Files.Application.Services.Elastic;
using Lapka.Files.Core.Events.Concrete;

namespace Lapka.Files.Application.Events.Internal.Handler
{
    public class PhotoCreatedEventHandler : IDomainEventHandler<PhotoCreated>
    {
        private readonly IPhotoElasticsearchUpdater _elasticsearchUpdater;

        public PhotoCreatedEventHandler(IPhotoElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }

        public async Task HandleAsync(PhotoCreated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Photo);
        }
    }
}