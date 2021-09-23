using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Files.Application.Events.External
{
    [Message("pets")]
    public class ShelterPetRemoved : IEvent
    {
        public Guid Id { get; }

        public ShelterPetRemoved(Guid id)
        {
            Id = id;
        }
    }
}