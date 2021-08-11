using System.IO;
using Convey.CQRS.Commands;

namespace Lapka.Files.Application.Commands
{
    public class UploadPhoto : ICommand
    {
        public string Path { get; }
        public byte[] Photo { get; }

        public UploadPhoto(string path, byte[] photo)
        {
            Path = path;
            Photo = photo;
        }
    }
}