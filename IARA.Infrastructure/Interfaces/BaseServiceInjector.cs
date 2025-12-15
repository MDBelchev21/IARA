using IARA.Persistance.Data;

namespace IARA.Infrastructure.Interfaces;

public class BaseServiceInjector
{
    public IARADbContext Db { get; }

    public BaseServiceInjector(IARADbContext db)
    {
        Db = db;
    }
}


