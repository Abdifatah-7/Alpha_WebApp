using Data.Contexts;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<RepositoryResult<bool>> CreateAsync(TEntity entity);
    Task<RepositoryResult<bool>> DeleteAsync(TEntity entity);
    Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> expression);
    Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[]? includes);
    Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<bool>> UpdateAsync(TEntity entity);
}

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _entity;

    protected BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _entity = context.Set<TEntity>();
    }



    //Create 
    public virtual async Task<RepositoryResult<bool>> CreateAsync(TEntity entity)
    {

        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null" };

        try
        {
            _entity.Add(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 201 };

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }


    }

    //Read

    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[]? includes)
    {
        try
        {
            IQueryable<TEntity> query = _entity;


            if (where != null)
                query = query.Where(where);


            if (sortBy != null)
            {
                query = orderByDescending
                    ? query.OrderByDescending(sortBy)
                    : query.OrderBy(sortBy);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var entities = await query.ToListAsync();
            return new RepositoryResult<IEnumerable<TEntity>>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving entities: {ex.Message}");
            return new RepositoryResult<IEnumerable<TEntity>> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }





    public virtual async Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            IQueryable<TEntity> query = _entity;

            if (where != null)
                query = query.Where(where);

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (sortBy != null)
            {
                query = orderByDescending
                    ? query.OrderByDescending(sortBy)
                    : query.OrderBy(sortBy);
            }

            var entities = await query.Select(selector).ToListAsync();

            return new RepositoryResult<IEnumerable<TSelect>> { Succeeded = true, StatusCode = 200, Result = entities };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving entities: {ex.Message}");
            return new RepositoryResult<IEnumerable<TSelect>> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }



    //read

    public virtual async Task<RepositoryResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
    {

        try
        {
            IQueryable<TEntity> query = _entity;

            // Apply includes if provided
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Apply the where condition
            var entity = await query.FirstOrDefaultAsync(where);

            if (entity == null)
                return new RepositoryResult<TEntity> { Succeeded = false, StatusCode = 404, Error = "Entity not found" };

            return new RepositoryResult<TEntity> { Succeeded = true, StatusCode = 200, Result = entity };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error finding entity: {ex.Message}");
            return new RepositoryResult<TEntity> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }





    //Update

    public virtual async Task<RepositoryResult<bool>> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null" };

        try
        {
            _entity.Update(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200, Result = true };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    //Delete

    public virtual async Task<RepositoryResult<bool>> DeleteAsync(TEntity entity)
    {


        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null" };

        try
        {
            _entity.Remove(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200, Result = true };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }


    }

    //Existing

    public virtual async Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            var exists = await _entity.AnyAsync(expression);
            return !exists
                ? new RepositoryResult<bool> { Succeeded = false, StatusCode = 404, Error = "Entity not Found" }
                : new RepositoryResult<bool> { Succeeded = true, StatusCode = 200, Result = exists };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error finding entities :: {ex.Message}");
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

}
