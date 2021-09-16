using System.IO;
using System.Threading.Tasks;
using Lapka.Files.Application.Services.Minios;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Minios.Options;
using Minio;
using Minio.Exceptions;

namespace Lapka.Files.Infrastructure.Minios.Services
{
    public class MinioServiceClient : IMinioServiceClient
    {
        private readonly MinioOptions _minioOptions;
        private readonly MinioClient _client;

        public MinioServiceClient(MinioOptions minioOptions)
        {
            _minioOptions = minioOptions;
            _client = new MinioClient(_minioOptions.Endpoint, _minioOptions.AccessKey, _minioOptions.SecretKey);
        }

        public async Task<byte[]> GetAsync(string path, BucketName bucket)
        {
            string bucketName = bucket switch
            {
                BucketName.PetPhotos => _minioOptions.PetBucketName,
                BucketName.ShelterPhotos => _minioOptions.ShelterBucketName,
                BucketName.UserPhotos => _minioOptions.UserBucketName,
                _ => throw new InvalidBucketNameException(bucket.ToString(), "Invalid bucket name")
            };
            
            await MakeSureBucketExistAsync(bucketName);

            await using MemoryStream ms = new MemoryStream();
            
            await _client.GetObjectAsync(bucketName, path, stream =>
            {
                stream.CopyTo(ms);
            });

            return ms.ToArray();
        }

        public async Task AddAsync(Photo photo, BucketName bucket)
        {
            string bucketName = bucket switch
            {
                BucketName.PetPhotos => _minioOptions.PetBucketName,
                BucketName.ShelterPhotos => _minioOptions.ShelterBucketName,
                BucketName.UserPhotos => _minioOptions.UserBucketName,
                _ => throw new InvalidBucketNameException(bucket.ToString(), "Invalid bucket name")
            };
            
            await MakeSureBucketExistAsync(bucketName);

            await _client.PutObjectAsync(bucketName, photo.Path, photo.Content,
                photo.Content.Length);
        }

        public async Task DeleteAsync(string photoPath, BucketName bucket)
        {
            string bucketName = bucket switch
            {
                BucketName.PetPhotos => _minioOptions.PetBucketName,
                BucketName.ShelterPhotos => _minioOptions.ShelterBucketName,
                BucketName.UserPhotos => _minioOptions.UserBucketName,
                _ => throw new InvalidBucketNameException(bucket.ToString(), "Invalid bucket name")
            };
            
            await MakeSureBucketExistAsync(bucketName);

            await _client.RemoveObjectAsync(bucketName, photoPath);
        }

        private async Task MakeSureBucketExistAsync(string bucketName)
        {
            bool bucketExists = await _client.BucketExistsAsync(bucketName);
            if (!bucketExists)
            {
                await _client.MakeBucketAsync(bucketName);
            }
        }
    }
}