namespace Odin.Auth.Domain.SeedWork
{
    public abstract class Tenant
    {
        public Guid TenantId { get; protected set; }

        protected Tenant(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }
}
