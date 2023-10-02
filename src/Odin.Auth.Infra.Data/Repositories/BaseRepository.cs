using Microsoft.AspNetCore.Http;

namespace Odin.Auth.Infra.Data.EF.Repositories
{
    public abstract class BaseRepository
    {        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUsername()
        {
            if (!string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext.User?.Identity?.Name))
                return _httpContextAccessor.HttpContext.User.Identity.Name!;
            else
                return "Anonymous";
        }
    }
}
