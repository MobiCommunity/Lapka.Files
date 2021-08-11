using System.IO;

namespace Lapka.Files.Core.ValueObjects
{
    public class Photo
    {
        public string Path { get; }
        public Stream Content { get; }
        
        public Photo(string path, Stream content)
        {
            Path = path;
            Content = content;
        }
    }
}