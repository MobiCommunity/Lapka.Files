using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Files.Application.Events.External
{
    [Message("identity")]
    public class ShelterRemoved : IEvent
    {
        public Guid ShelterId { get; }

        public ShelterRemoved(Guid shelterId)
        {
            ShelterId = shelterId;
        }
    }
}