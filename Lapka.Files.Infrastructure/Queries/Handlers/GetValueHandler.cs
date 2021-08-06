using Convey.CQRS.Queries;
using System.Threading.Tasks;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Queries;
using Lapka.Files.Application.Services;

namespace Lapka.Files.Infrastructure.Queries.Handlers
{
    public class GetValueHandler : IQueryHandler<GetValue, ValueDto>
    {
        private readonly IValueRepository _service;

        public GetValueHandler(IValueRepository service)
        {
            _service = service;
        }

        public async Task<ValueDto> HandleAsync(GetValue query)
        {
            return await _service.GetById(query.Id);
        }
    }
}
