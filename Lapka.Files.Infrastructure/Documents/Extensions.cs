using System.IO;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Infrastructure.Documents
{
    public static class Extensions
    {
        public static PhotoDocument AsDocument(this Photo photo)
        {
            return new PhotoDocument(photo.Id.Value, photo.Path);
        }
        
        public static Photo AsDocument(this PhotoDocument photo)
        {
            return new Photo(photo.Id, photo.PhotoPath, Stream.Null);
        }
    }
}