using System;
using System.IO;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Queries;
using Lapka.Files.Core.ValueObjects;
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
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, BucketName bucketName)
        {
            PhotoDto photoDto = await _queryDispatcher.QueryAsync(new GetPhoto
            {
                Id = id,
                BucketName = bucketName
            });
            
            return File(photoDto.Content, "image/png");
        }
    }
}