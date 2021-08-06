using Convey.CQRS.Events;
using System.Collections.Generic;
using Lapka.Files.Core.Events.Abstract;

namespace Lapka.Files.Application.Services
{
    public interface IDomainToIntegrationEventMapper
    {
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
        IEvent Map(IDomainEvent @event);
        
    }
}