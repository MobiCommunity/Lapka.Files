using Lapka.Files.Core.Entities;
using Lapka.Files.Core.Events.Abstract;

namespace Lapka.Files.Core.Events.Concrete
{
    public class ValueCreated : IDomainEvent
    {
        public Value Value { get; }

        public ValueCreated(Value value)
        {
            Value = value;
        }
    }
}