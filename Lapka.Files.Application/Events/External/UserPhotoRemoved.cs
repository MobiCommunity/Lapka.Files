using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Files.Application.Events.External
{
    [Message("identity")]
    public class UserPhotoRemoved : IEvent
    {
        public string PhotoPath { get; }

        public UserPhotoRemoved(string photoPath)
        {
            PhotoPath = photoPath;
        }
    }
}