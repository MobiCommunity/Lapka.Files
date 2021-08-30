using System;
using Convey.CQRS.Queries;
using Lapka.Files.Application.Dto;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Queries
{
    public class GetPhotoPath : IQuery<PhotoPathDto>
    {
        public string Id { get; set; }
        public BucketName BucketName { get; set; }
    }
}