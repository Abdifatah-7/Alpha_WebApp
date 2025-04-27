using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public interface IAppUserRepository : IBaseRepository<AppUserEntity>
{
}
public class AppUserRepository(ApplicationDbContext context) : BaseRepository<AppUserEntity>(context), IAppUserRepository
{
}
