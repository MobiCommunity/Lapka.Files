namespace Lapka.Files.Application.Exceptions
{
    public class PhotoIsPrivateException : AppException
    {
        public string PhotoPath { get; }
        public PhotoIsPrivateException(string path) : base($"Photo with path {path} is private")
        {
            PhotoPath = path;
        }
        public override string Code => "photo_is_private";
    }
}