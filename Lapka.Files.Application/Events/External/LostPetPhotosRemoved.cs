using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Files.Application.Events.External
{
    [Message("pets")]
    public class LostPetPhotosRemoved : IEvent
    {
        public IEnumerable<string> PhotoPaths { get; }

        public LostPetPhotosRemoved(IEnumerable<string> photoPaths)
        { 
            PhotoPaths = photoPaths;
        }
    }
}