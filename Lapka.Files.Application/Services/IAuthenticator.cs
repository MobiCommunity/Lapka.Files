using Microsoft.AspNetCore.Http;

namespace Lapka.Files.Application.Services
{
    public interface IAuthenticator
    {
        public void Authenticate(HttpContext ctx);
    }
}