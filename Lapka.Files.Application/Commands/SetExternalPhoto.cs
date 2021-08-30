using Convey.CQRS.Commands;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands
{
    public class SetExternalPhoto : ICommand
    {
        public string Id { get; }
        public string OldName { get; }
        public string NewName { get; }
        public BucketName BucketName { get; }

        public SetExternalPhoto(string id, string oldName, string newName, BucketName bucketName)
        {
            Id = id;
            OldName = oldName;
            NewName = newName;
            BucketName = bucketName;
        }
    }
}