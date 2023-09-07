using Microsoft.AspNetCore.Mvc;

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
    }
}
