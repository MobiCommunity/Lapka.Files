using System.IO;
using Lapka.Files.Application.Dto;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static PhotoDocument AsDocument(this Photo photo)
        {
            return new PhotoDocument
            {
                Id = photo.Id.Value,
                Path = photo.Path,
                IsPublic = photo.IsPublic,
                UserId = photo.UserId
            };
        }
        
        public static Photo AsBusiness(this PhotoDocument photo)
        {
            return new Photo(photo.Id, photo.Path, photo.IsPublic, photo.UserId, Stream.Null);
        }
        
        public static PhotoPathDto AsDto(this PhotoDocument photo)
        {
            return new PhotoPathDto
            {
                Path = photo.Path
            };
        }
    }
}