using System;
using System.IO;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Files.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
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
            PhotoDto photoDto = await _queryDispatcher.QueryAsync(new GetPhoto
            {
                Path = path
            });
            
            return File(photoDto.Content, "image/png");
        }
    }
}