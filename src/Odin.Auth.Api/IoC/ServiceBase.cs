namespace Odin.Auth.Api.IoC
{
    public abstract class ServiceBase
    {
        internal static T GetInstance<T>()
            => (T)Activator.CreateInstance(typeof(T));

        protected internal void Add(IServiceCollection services)
        {
            HttpClient(services);
            Scoped(services);
            Singleton(services);
            Transient(services);
        }

        protected virtual void HttpClient(IServiceCollection services)
        {
        }

        protected virtual void Scoped(IServiceCollection services)
        {
        }

        protected virtual void Singleton(IServiceCollection services)
        {
        }

        protected virtual void Transient(IServiceCollection services)
        {
        }
    }

}
