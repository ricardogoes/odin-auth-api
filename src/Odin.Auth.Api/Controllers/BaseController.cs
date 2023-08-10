using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Domain.Models;

namespace Odin.Auth.Api.Controllers
{
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
        private readonly ILogger _logger;
        
        public BaseController(
            ILogger logger)
        {
            _logger = logger;
        }

        protected IActionResult HandleError(Exception ex)
        {
            return StatusCode(500, new ApiResponse(ApiResponseState.Failed, "An error ocurred, please try again. If the error persists contact the System Administrator"));
        }
    }

}
