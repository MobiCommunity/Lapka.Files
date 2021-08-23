using Convey.CQRS.Commands;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands
{
    public class DeletePhoto : ICommand
    {
        public string Path { get; }
        public BucketName BucketName { get; }

        public DeletePhoto(string path, BucketName bucketName)
        {
            Path = path;
            BucketName = bucketName;
        }
    }
}