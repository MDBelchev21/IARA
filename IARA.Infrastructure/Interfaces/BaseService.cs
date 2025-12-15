using IARA.Persistance.Data;

namespace IARA.Infrastructure.Interfaces;

public abstract class BaseService
{
    protected IARADbContext Db { get; }

    protected BaseService(BaseServiceInjector injector)
    {
        Db = injector.Db;
    }
}


