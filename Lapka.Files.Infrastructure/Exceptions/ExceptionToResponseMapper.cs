using Convey.WebApi.Exceptions;
using System;
using System.Net;
using Lapka.Files.Application.Exceptions;
using Lapka.Files.Core.Exceptions;
using Lapka.Files.Core.Exceptions.Abstract;

namespace Lapka.Files.Infrastructure.Exceptions
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DomainException ex => ex switch
                {
                    InvalidAggregateIdException invalidAggregateIdException => new ExceptionResponse(new
                    {
                        code = invalidAggregateIdException.Code,
                        reason = invalidAggregateIdException.Message
                    }, HttpStatusCode.BadRequest),
                    _ => new ExceptionResponse(new {code = ex.Code, reason = ex.Message},
                        HttpStatusCode.BadRequest),
                },
                
                AppException ex => ex switch
                {
                    CannotConnectToMinioException cannotConnectToMinioException => 
                        new ExceptionResponse(new
                        {
                            code = cannotConnectToMinioException.Code,
                            reason = cannotConnectToMinioException.Message
                        },
                        HttpStatusCode.InternalServerError),
                    InvalidBucketNameException invalidBucketNameException => 
                        new ExceptionResponse(new
                            {
                                code = invalidBucketNameException.Code,
                                reason = invalidBucketNameException.Message
                            },
                            HttpStatusCode.BadRequest),
                    InvalidPhotoIdException invalidPhotoIdException => 
                        new ExceptionResponse(new
                            {
                                code = invalidPhotoIdException.Code,
                                reason = invalidPhotoIdException.Message
                            },
                            HttpStatusCode.BadRequest),
                    PhotoNotFoundException photoNotFoundException => 
                        new ExceptionResponse(new
                            {
                                code = photoNotFoundException.Code,
                                reason = photoNotFoundException.Message
                            },
                            HttpStatusCode.BadRequest),
                    _ => new ExceptionResponse(
                        new
                        {
                            code = ex.Code,
                            reason = ex.Message
                        },
                        HttpStatusCode.BadRequest)
                },

                _ => new ExceptionResponse(new
                    {
                        code = "error", reason = "There was an error."
                    },
                    HttpStatusCode.BadRequest)
            };
    }
}