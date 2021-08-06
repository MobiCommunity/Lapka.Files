using Convey.CQRS.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Files.Application.Services;

namespace Lapka.Files.Infrastructure.Services
{
    public class DummyMessageBroker : IMessageBroker
    {
        public Task PublishAsync(params IEvent[] events)
        {
            return Task.CompletedTask;
        }

        public Task PublishAsync(IEnumerable<IEvent> events)
        {
            return Task.CompletedTask;
        }
    }
}