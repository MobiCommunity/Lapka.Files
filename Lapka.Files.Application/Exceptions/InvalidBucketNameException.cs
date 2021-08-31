namespace Lapka.Files.Application.Exceptions
{
    public class InvalidBucketNameException : AppException
    {
        public string BucketName { get; }
        public InvalidBucketNameException(string bucketName) : base($"Invalid bucket name: {bucketName}")
        {
            BucketName = bucketName;
        }

        public override string Code => "invalid_bucket_name";
    }
}