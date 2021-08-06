using Convey.CQRS.Commands;
using System.Threading.Tasks;
using Lapka.Files.Application.Services;
using Lapka.Files.Core.Entities;

namespace Lapka.Files.Application.Commands.Handlers
{
    public class CreateValueHandler : ICommandHandler<CreateValue>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IValueRepository _valueRepository;

        public CreateValueHandler(IEventProcessor eventProcessor, IValueRepository valueRepository)
        {
            _eventProcessor = eventProcessor;
            _valueRepository = valueRepository;
        }

        public async Task HandleAsync(CreateValue command)
        {
            Value value = Value.Create(command.Id,command.Name,command.Description);
            
            await _valueRepository.AddValue(value);
            
            await _eventProcessor.ProcessAsync(value.Events);
        }
    }
}