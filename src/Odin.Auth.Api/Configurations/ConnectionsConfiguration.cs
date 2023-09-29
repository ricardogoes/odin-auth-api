using Microsoft.EntityFrameworkCore;
using Odin.Auth.Domain.Models.AppSettings;
using Odin.Auth.Infra.Data.EF;

namespace Odin.Auth.Api.Configurations
{
    public static class ConnectionsConfiguration
    {
        public static IServiceCollection AddAppConnections(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddDbConnection(appSettings);
            return services;
        }

        private static IServiceCollection AddDbConnection(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddDbContext<OdinMasterDbContext>(options =>
            {
                options.UseNpgsql(appSettings.ConnectionStringsSettings!.OdinMasterDbConnection);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }
    }
}
