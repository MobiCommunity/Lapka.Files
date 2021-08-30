namespace Lapka.Files.Application.Exceptions
{
    public class PhotoNotFoundException : AppException
    {
        public string Id { get; }
        public PhotoNotFoundException(string id) : base($"Photo not found id: {id}")
        {
            Id = id;
        }
        public override string Code => "photo_not_found";
    }
}