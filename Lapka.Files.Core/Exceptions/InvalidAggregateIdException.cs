using System;
using Lapka.Files.Core.Exceptions.Abstract;

namespace Lapka.Files.Core.Exceptions
{
    public class InvalidAggregateIdException : DomainException
    {
        public override string Code { get; } = "invalid_aggregate_id";
        public Guid Id { get; }

        public InvalidAggregateIdException(Guid id) : base($"Invalid aggregate id: {id}")
            => Id = id;
    }

}