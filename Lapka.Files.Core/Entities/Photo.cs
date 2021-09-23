using System;
using System.IO;
using Lapka.Files.Core.Events.Concrete;

namespace Lapka.Files.Core.Entities
{
    public class Photo : AggregateRoot
    {
        public Guid UserId { get; }
        public bool IsPublic { get; }
        public string Path { get; }
        public Stream Content { get; }

        public Photo(Guid id, string path, bool isPublic, Guid userId, Stream content)
        {
            Id = new AggregateId(id);
            Path = path;
            IsPublic = isPublic;
            UserId = userId;
            Content = content;
        }

        public static Photo Create(Guid id, string path, bool isPublic, Guid userId, Stream content)
        {
            Photo photo = new Photo(id, path, isPublic, userId, content);

            photo.AddEvent(new PhotoCreated(photo));
            return photo;
        }
    }
}