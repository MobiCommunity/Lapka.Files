using System;
using System.Threading.Tasks;
using Lapka.Files.Application.Dto;
using Lapka.Files.Core.Entities;

namespace Lapka.Files.Application.Services
{
    public interface IValueRepository
    {
        Task AddValue(Value value);
        Task<ValueDto> GetById(Guid id);
    }
}