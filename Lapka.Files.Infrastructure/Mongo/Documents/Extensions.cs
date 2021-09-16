using System.IO;
using Lapka.Files.Application.Dto;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static PhotoDocument AsDocument(this Photo photo)
        {
            return new PhotoDocument(photo.Id.Value, photo.Path);
        }
        
        public static Photo AsBusiness(this PhotoDocument photo)
        {
            return new Photo(photo.Id, photo.PhotoPath, Stream.Null);
        }
        
        public static PhotoPathDto AsDto(this PhotoDocument photo)
        {
            return new PhotoPathDto
            {
                Path = photo.PhotoPath
            };
        }
    }
}