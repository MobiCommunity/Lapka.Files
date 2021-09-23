namespace Lapka.Files.Application.Exceptions
{
    public class InvalidUserIdException : AppException
    {
        public string UserId { get; }
        public InvalidUserIdException(string uesrId) : base($"User id is invalid: {uesrId}")
        {
            UserId = uesrId;
        }

        public override string Code => "invalid_user_id";
    }
}