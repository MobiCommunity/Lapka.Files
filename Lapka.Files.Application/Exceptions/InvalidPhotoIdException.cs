namespace Lapka.Files.Application.Exceptions
{
    public class InvalidPhotoIdException : AppException
    {
        public string Id { get; }
        public InvalidPhotoIdException(string id) : base($"Invalid photo id: {id}")
        {
            Id = id;
        }

        public override string Code => "invalid_photo_id";
    }
}