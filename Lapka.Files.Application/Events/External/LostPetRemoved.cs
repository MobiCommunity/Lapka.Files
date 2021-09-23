using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Files.Application.Events.External
{
    [Message("pets")]
    public class LostPetRemoved : IEvent
    {
        public Guid Id { get; }

        public LostPetRemoved(Guid id)
        {
            Id = id;
        }
    }
}