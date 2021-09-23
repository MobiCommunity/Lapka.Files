using System;
using Convey.CQRS.Commands;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands
{
    public class DeletePhoto : ICommand
    {
        public string PhotoPath { get; }
        public string UserId { get; }
        public BucketName BucketName { get; }

        public DeletePhoto(string photoPath, string userId, BucketName bucketName)
        {
            PhotoPath = photoPath;
            UserId = userId;
            BucketName = bucketName;
        }
    }
}