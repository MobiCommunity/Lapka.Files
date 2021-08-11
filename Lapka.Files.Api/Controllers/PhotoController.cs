using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Files.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Files.Api.Controllers
{
    [ApiController]
    [Route("api/values")]
    public class PhotoController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public PhotoController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }
        
        [HttpGet("{path}")]
        public async Task<IActionResult> Get(string path)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetPhoto
            {
                Path = path
            }));
        }
    }
}