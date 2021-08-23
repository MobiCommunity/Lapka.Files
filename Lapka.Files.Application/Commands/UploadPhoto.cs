using System.IO;
using Convey.CQRS.Commands;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Commands
{
    public class UploadPhoto : ICommand
    {
        public string Path { get; }
        public byte[] Photo { get; }
        public BucketName BucketName { get; }

        public UploadPhoto(string path, byte[] photo, BucketName bucketName)
        {
            Path = path;
            Photo = photo;
            BucketName = bucketName;
        }
    }
}