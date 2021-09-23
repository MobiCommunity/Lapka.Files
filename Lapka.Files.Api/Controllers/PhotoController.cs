using System;
using System.IO;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Files.Application.Dto;
using Lapka.Files.Application.Queries;
using Lapka.Files.Core.ValueObjects;
using Lapka.Files.Infrastructure;
using Microsoft.AspNetCore.Http;
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
        
        /// <summary>
        /// Gets photo by path.
        /// </summary>
        /// <returns>Photo</returns>
        /// <response code="200">If successfully got photo, returns photo as image</response>
        /// <response code="404">If photo was not found</response>
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [HttpGet("{photoPath}")]
        public async Task<IActionResult> Get(string photoPath, BucketName bucketName)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            
            PhotoDto photoDto = await _queryDispatcher.QueryAsync(new GetPhoto
            {
                PhotoPath = photoPath,
                UserId = userId,
                BucketName = bucketName
            });
            
            return File(photoDto.Content, "image/png");
        }
    }
}