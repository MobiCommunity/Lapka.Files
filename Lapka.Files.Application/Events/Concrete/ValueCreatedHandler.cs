using System;
using System.Threading.Tasks;
using Lapka.Files.Application.Events.Abstract;
using Lapka.Files.Core.Events.Concrete;

namespace Lapka.Files.Application.Events.Concrete
{
    public class ValueCreatedHandler : IDomainEventHandler<ValueCreated>
    {

        public Task HandleAsync(ValueCreated @event)
        {
            Console.WriteLine($"i caught {@event.Value.Name}");
            return Task.CompletedTask;
        }
    }
}