using System.Threading.Tasks;
using Lapka.Files.Core.Entities;
using Lapka.Files.Core.ValueObjects;

namespace Lapka.Files.Application.Services.Elastic
{
    public interface IPhotoElasticsearchUpdater
    {
        Task InsertAndUpdateDataAsync(Photo photo);
        Task DeleteDataAsync(Photo photo);
    }
}