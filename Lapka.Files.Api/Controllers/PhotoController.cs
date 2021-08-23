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

        [HttpGet("{path}")]
        public async Task<IActionResult> Get(string path, BucketName bucketName)
        {
            PhotoDto photoDto = await _queryDispatcher.QueryAsync(new GetPhoto
            {
                Path = path,
                BucketName = bucketName
            });
            
            return File(photoDto.Content, "image/png");
        }
    }
}