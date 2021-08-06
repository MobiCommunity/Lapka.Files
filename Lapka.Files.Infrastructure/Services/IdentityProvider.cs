using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Lapka.Files.Application.Services;

namespace Lapka.Files.Infrastructure.Services
{
    public class IdValueProvider : IIdValueProvider
    {
        public Guid GetIdValue(HttpContext ctx) 
            => Guid.Parse(GetIdValueAsString(ctx));

        public string GetIdValueAsString(HttpContext ctx) 
            => ctx.Request.Headers["Authorization"].FirstOrDefault();

    }
}