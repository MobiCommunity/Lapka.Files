using System;
using System.IO;
using Convey.CQRS.Commands;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands
{
    public class UploadPhoto : ICommand
    {
        public string UserId { get; }
        public bool IsPublic { get; }
        public string Name { get; }
        public byte[] Photo { get; }
        public BucketName BucketName { get; }

        public UploadPhoto(string userId, bool isPublic, string name, byte[] photo, BucketName bucketName)
        {
            UserId = userId;
            IsPublic = isPublic;
            Name = name;
            Photo = photo;
            BucketName = bucketName;
        }
    }
}