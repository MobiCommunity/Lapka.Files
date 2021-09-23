using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Files.Application.Events.External
{
    [Message("pets")]
    public class ShelterPetPhotosRemoved : IEvent
    {
        public IEnumerable<string> PhotoPaths { get; }

        public ShelterPetPhotosRemoved(IEnumerable<string> photoPaths)
        { 
            PhotoPaths = photoPaths;
        }
    }
}