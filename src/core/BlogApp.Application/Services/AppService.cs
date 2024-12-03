using BlogApp.Domain.Ports;

namespace BlogApp.Application.Services;

public abstract class AppService
{
    private readonly IUnitOfWork _unitOfWork;

    protected AppService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected async Task Commit()
    {
        await _unitOfWork.Commit();
    }
}