using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
}
public class ProjectRepository(ApplicationDbContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
}
