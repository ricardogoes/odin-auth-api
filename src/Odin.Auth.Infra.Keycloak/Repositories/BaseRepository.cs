using Microsoft.AspNetCore.Http;

namespace Odin.Auth.Infra.Keycloak.Repositories
{
    public abstract class BaseRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetTenantId()
        {
            if (!string.IsNullOrWhiteSpace(_httpContextAccessor?.HttpContext?.Request?.Headers["X-TENANT-ID"]))
                return Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers["X-TENANT-ID"]!);
            else
                // Used for tests only
                return Guid.Parse("5F9B7808-803F-4985-9996-6EBA9003F9CD");
        }

            public string GetCurrentUsername()
        {
            if (!string.IsNullOrWhiteSpace(_httpContextAccessor?.HttpContext?.User?.Identity?.Name))
                return _httpContextAccessor.HttpContext.User.Identity.Name!;
            else
                return "Anonymous";
        }
    }
}
