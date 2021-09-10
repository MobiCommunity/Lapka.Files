using Lapka.Files.Core.Events.Abstract;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Core.Events.Concrete
{
    public class PhotoCreated : IDomainEvent
    {
        public Photo Photo { get; }

        public PhotoCreated(Photo photo)
        {
            Photo = photo;
        }
    }
}