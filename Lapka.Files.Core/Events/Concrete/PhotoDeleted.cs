using Lapka.Files.Core.Events.Abstract;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Core.Events.Concrete
{
    public class PhotoDeleted : IDomainEvent
    {
        public Photo Photo { get; }

        public PhotoDeleted(Photo photo)
        {
            Photo = photo;
        }
    }
}