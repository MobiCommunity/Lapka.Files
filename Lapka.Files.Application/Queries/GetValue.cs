using Convey.CQRS.Queries;
using System;
using Lapka.Files.Application.Dto;

namespace Lapka.Files.Application.Queries
{
    public class GetValue : IQuery<ValueDto>
    {
        public Guid Id { get; set; }

    }
}
