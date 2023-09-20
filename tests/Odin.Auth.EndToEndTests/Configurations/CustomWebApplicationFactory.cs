using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

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

            base.ConfigureWebHost(builder);
        }
    }
}
