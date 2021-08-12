using System.IO;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure.Options;
using Minio;

namespace Lapka.Files.Infrastructure.Services
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

        public async Task<byte[]> GetAsync(string path)
        {
            await MakeSureBucketExistAsync(_minioOptions.PetBucketName);

            await using var ms = new MemoryStream();
            
            await _client.GetObjectAsync(_minioOptions.PetBucketName, path, stream =>
            {
                stream.CopyTo(ms);
            });

            return ms.ToArray();
        }

        public async Task AddAsync(Photo photo)
        {
            await MakeSureBucketExistAsync(_minioOptions.PetBucketName);

            await _client.PutObjectAsync(_minioOptions.PetBucketName, photo.Path, photo.Content,
                photo.Content.Length);
        }

        public async Task DeleteAsync(string photoPath)
        {
            await MakeSureBucketExistAsync(_minioOptions.PetBucketName);

            await _client.RemoveObjectAsync(_minioOptions.PetBucketName, photoPath);
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