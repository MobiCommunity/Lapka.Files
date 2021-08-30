using System;
using Convey.Types;

namespace Lapka.Files.Infrastructure.Documents
{
    public class PhotoDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; }
        public string PhotoPath { get; }

        public PhotoDocument(Guid id, string photoPath)
        {
            Id = id;
            PhotoPath = photoPath;
        }
    }
}