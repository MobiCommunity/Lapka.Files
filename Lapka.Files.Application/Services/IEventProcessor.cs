using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Files.Core.Events.Abstract;

namespace Lapka.Files.Application.Services
{
    public interface IEventProcessor
    {
        Task ProcessAsync(IEnumerable<IDomainEvent> events);
    }
}