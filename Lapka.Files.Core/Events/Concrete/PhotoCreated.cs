using Lapka.Files.Core.Events.Abstract;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Core.Events.Concrete
{
    public class PhotoCreated : IDomainEvent
    {
        public Photo photo { get; }

        public PhotoCreated(Photo photo)
        {
            this.photo = photo;
        }
    }
}