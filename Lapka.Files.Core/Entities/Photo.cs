using System;
using System.IO;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.Events.Concrete;

namespace Lapka.Files.Core.ValueObjects
{
    public class Photo : AggregateRoot
    {
        public string Path { get; }
        public Stream Content { get; }

        public Photo(Guid id, string path, Stream content)
        {
            Id = new AggregateId(id);
            Path = path;
            Content = content;
        }

        public static Photo Create(Guid id, string path, Stream content)
        {
            Photo photo = new Photo(id, path, content);
            
            photo.AddEvent(new PhotoCreated(photo));
            return photo;
        }

        public void Delete()
        {
            AddEvent(new PhotoDeleted(this));
        }
    }
}