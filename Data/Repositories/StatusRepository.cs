using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public interface IStatusRepository : IBaseRepository<StatusEntity>
{
}
public class StatusRepository(ApplicationDbContext context) : BaseRepository<StatusEntity>(context), IStatusRepository
{
}
