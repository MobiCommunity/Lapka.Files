namespace Lapka.Files.Application.Exceptions
{
    public class PhotoDoesNotBelongToUserException : AppException
    {
        public string UserId { get; }
        public PhotoDoesNotBelongToUserException(string userId) : base($"Photo does not belong to user {userId}")
        {
            UserId = userId;
        }
        public override string Code => "photo_does_belong_to_user";
    }
}