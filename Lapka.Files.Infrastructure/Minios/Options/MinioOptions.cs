namespace Lapka.Files.Infrastructure.Minios.Options
{
    public class MinioOptions
    {
        public string Endpoint { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public string PetBucketName { get; set; }
        public string UserBucketName { get; set; }
        public string ShelterBucketName { get; set; }
    }
}