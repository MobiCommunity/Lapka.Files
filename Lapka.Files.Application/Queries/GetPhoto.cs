using System;
using Convey.CQRS.Queries;
using Lapka.Files.Application.Dto;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Queries
{
    public class GetPhoto : IQuery<PhotoDto>
    {
        public Guid Id { get; set; }
        public BucketName BucketName { get; set; }
    }
}