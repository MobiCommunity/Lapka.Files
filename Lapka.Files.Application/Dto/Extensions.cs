using Lapka.Files.Application.Commands;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Dto
{
    public static class Extensions
    {
        public static string GetFileExtension(this UploadPhoto uploadPhoto) =>
            uploadPhoto.Name.Contains('.') ? uploadPhoto.Name.Split('.')[1] : string.Empty;
    }
}