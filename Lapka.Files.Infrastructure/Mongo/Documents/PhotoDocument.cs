using System;
using Convey.Types;
using MongoDB.Bson.Serialization.Attributes;

namespace Lapka.Files.Infrastructure.Mongo.Documents
{
    public class PhotoDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Path { get; set;}
        public bool IsPublic { get; set; }
        public Guid UserId { get; set;}

    }
}