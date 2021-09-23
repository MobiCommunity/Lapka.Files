using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Files.Application.Events.External
{
    [Message("identity")]
    public class UserRemoved : IEvent
    {
        public Guid UserId { get; }

        public UserRemoved(Guid userId)
        {
            UserId = userId;
        }
    }
}