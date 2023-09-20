namespace Odin.Auth.Domain.SeedWork
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        public Entity(Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }
    }
}
