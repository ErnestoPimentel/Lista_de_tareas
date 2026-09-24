namespace TaskManager.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    //Constructor protegido para evitar crear instancias de Entity directamente.
    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}
