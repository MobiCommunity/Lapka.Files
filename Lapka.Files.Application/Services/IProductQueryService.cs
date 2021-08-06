using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Files.Application.Dto;

namespace Lapka.Files.Application.Services
{
    public interface IValueQueryService
    {
        Task<ValueDto> GetValueById(Guid id);
        Task<IEnumerable<ValueDto>> GetAllValues();
    }
}