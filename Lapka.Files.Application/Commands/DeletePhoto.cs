using Convey.CQRS.Commands;

namespace Lapka.Files.Application.Commands
{
    public class DeletePhoto : ICommand
    {
        public string Path { get; }

        public DeletePhoto(string path)
        {
            Path = path;
        }
    }
}