using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;

namespace Odin.Auth.Api.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;        
        
        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICustomerRepository customerRepository)
        {
            if (context.Request.Path.HasValue && !context.Request.Path.Value!.Contains("customer"))
            {
                context.Request.Headers.TryGetValue("X-TENANT-ID", out var tenantId);

                if (string.IsNullOrWhiteSpace(tenantId))
                    throw new BadRequestException("Invalid tenant");
                                
                /*var customer = await customerRepository.FindByIdAsync(new Guid(tenantId!), new CancellationToken());
                if (customer is null)
                    throw new BadRequestException("Invalid tenant");*/           
                
            }

            await _next(context);
        }
    }
}
