using System.Threading.Tasks;
using Lapka.Files.Core.Events.Abstract;

namespace Lapka.Files.Application.Events.Abstract
{
    public interface IDomainEventHandler<in T> where T : class, IDomainEvent
    {
        Task HandleAsync(T @event);
    }
}