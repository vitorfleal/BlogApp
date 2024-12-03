namespace BlogApp.Domain.Base.Models;

public abstract class Entity
{
    protected Entity(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        CreatedDate = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public DateTime CreatedDate { get; private set; }
}