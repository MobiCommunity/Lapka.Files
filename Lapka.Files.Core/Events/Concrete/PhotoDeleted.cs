using Lapka.Files.Core.Events.Abstract;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Core.Events.Concrete
{
    public class PhotoDeleted : IDomainEvent
    {
        public Photo photo { get; }

        public PhotoDeleted(Photo photo)
        {
            this.photo = photo;
        }
    }
}