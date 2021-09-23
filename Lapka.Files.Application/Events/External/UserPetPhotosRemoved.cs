using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Files.Application.Events.External
{
    [Message("pets")]
    public class UserPetPhotosRemoved : IEvent
    {
        public IEnumerable<string> PhotoPaths { get; }

        public UserPetPhotosRemoved(IEnumerable<string> photoPaths)
        { 
            PhotoPaths = photoPaths;
        }
    }
}