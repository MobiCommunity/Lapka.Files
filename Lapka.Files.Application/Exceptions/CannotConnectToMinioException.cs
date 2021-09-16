using System;

namespace Lapka.Files.Application.Exceptions
{
    public class CannotConnectToMinioException : AppException
    {
        public Exception Exception { get; }
        public string ErrorMessage { get; }

        public CannotConnectToMinioException(Exception exception, string errorMessage) : base(
            $"Cannot connect to minio with message: {errorMessage} and error: {exception.Message}")
        {
            Exception = exception;
            ErrorMessage = errorMessage;
        }

        public override string Code => "cannot_connect_to_minio";
    }
}