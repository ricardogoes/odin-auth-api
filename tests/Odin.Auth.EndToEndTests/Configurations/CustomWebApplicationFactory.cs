using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Odin.Auth.Infra.Data.EF;

namespace Odin.Auth.EndToEndTests.Configurations
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var environment = "EndToEndTest";
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
            builder.UseEnvironment(environment);

            builder.ConfigureServices(services =>
            {
                var dbOptions = services.FirstOrDefault(x => x.ServiceType == typeof(DbContextOptions<OdinMasterDbContext>));
                if (dbOptions != null)
                    services.Remove(dbOptions);

                services.AddDbContext<OdinMasterDbContext>(options =>
                {
                    options.UseInMemoryDatabase("e2e-tests-db");
                });
            });

            base.ConfigureWebHost(builder);
        }
    }
}
