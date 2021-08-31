using System;
using System.IO;
using Convey.CQRS.Commands;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands
{
    public class UploadPhoto : ICommand
    {
        public string Id { get; }
        public string Name { get; set; }
        public byte[] Photo { get; }
        public BucketName BucketName { get; }

        public UploadPhoto(string id, string name, byte[] photo, BucketName bucketName)
        {
            Id = id;
            Name = name;
            Photo = photo;
            BucketName = bucketName;
        }
    }
}