namespace BlogApp.Domain.Ports;

public interface IUnitOfWork
{
    Task Commit();
}