using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Files.Application.Events.External
{
    [Message("pets")]
    public class UserPetRemoved : IEvent
    {
        public Guid Id { get; }

        public UserPetRemoved(Guid id)
        {
            Id = id;
        }
    }
}