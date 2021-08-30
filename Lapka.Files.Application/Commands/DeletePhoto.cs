using System;
using Convey.CQRS.Commands;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands
{
    public class DeletePhoto : ICommand
    {
        public string Id { get; }
        public BucketName BucketName { get; }

        public DeletePhoto(string id, BucketName bucketName)
        {
            Id = id;
            BucketName = bucketName;
        }
    }
}