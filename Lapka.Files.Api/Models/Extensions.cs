using Lapka.Files.Application.Exceptions;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Api.Models
{
    public static class Extensions
    {
        public static BucketName AsValueObject(this UploadPhotoRequest.Types.Bucket bucket)
        {
            return bucket switch
            {
                UploadPhotoRequest.Types.Bucket.PetPhotos => BucketName.PetPhotos,
                UploadPhotoRequest.Types.Bucket.ShelterPhotos => BucketName.ShelterPhotos,
                UploadPhotoRequest.Types.Bucket.UserPhotos => BucketName.UserPhotos,
                _ => throw new InvalidBucketNameException(bucket.ToString())
            };
        }
        
        public static BucketName AsValueObject(this DeletePhotoRequest.Types.Bucket bucket)
        {
            return bucket switch
            {
                DeletePhotoRequest.Types.Bucket.PetPhotos => BucketName.PetPhotos,
                DeletePhotoRequest.Types.Bucket.ShelterPhotos => BucketName.ShelterPhotos,
                DeletePhotoRequest.Types.Bucket.UserPhotos => BucketName.UserPhotos,
                _ => throw new InvalidBucketNameException(bucket.ToString())
            };
        }
    }
}