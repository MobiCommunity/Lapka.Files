using Convey.CQRS.Queries;
using Lapka.Files.Application.Dto;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Queries
{
    public class GetPhoto : IQuery<PhotoDto>
    {
        public string Path { get; set; }
        public BucketName BucketName { get; set; }
    }
}