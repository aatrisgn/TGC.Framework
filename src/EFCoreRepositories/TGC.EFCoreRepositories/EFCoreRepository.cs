using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TGC.EFCoreRepositories;
/// <summary>
/// Base repository which contains basic functionality for CRUD operations
/// </summary>
/// <typeparam name="T"></typeparam>
public class EFCoreRepository<T> : IEFCoreRepository<T> where T : class, IEFCoreDTO, new()
{
	private readonly EFCoreContext _EFCoreContext;
	/// <summary>
	/// Entity specific DBSet
	/// </summary>
	/// <typeparam name="T">Entity type</typeparam>
	protected DbSet<T> Context;

	public EFCoreRepository(EFCoreContext EFCoreContext)
	{
		_EFCoreContext = EFCoreContext;
		Context = _EFCoreContext.Set<T>();
	}

	public async Task<T> CreateAsync(T entity)
	{
		if (entity != null)
		{
			var newEntity = await Context.AddAsync(entity);
			await _EFCoreContext.SaveChangesAsync();
			return newEntity.Entity;
		}
		throw new NullReferenceException("You cannot create a new entity by parsing null");
	}

	public async Task SoftDeleteAsync(Guid id)
	{
		var entityToRemove = await GetByIdAsync(id);
		await SoftDeleteAsync(entityToRemove);
	}

	public async Task SoftDeleteAsync(T entity)
	{
		entity.Active = false;
		await this.UpdateAsync(entity);
	}

	public async Task HardDeleteAsync(Guid id)
	{
		var entityToRemove = await GetByIdAsync(id);
		await HardDeleteAsync(entityToRemove);
	}

	public async Task HardDeleteAsync(T entity)
	{
		Context.Remove(entity);
		await _EFCoreContext.SaveChangesAsync();
	}

	public async Task<List<Guid>> GetAllIdsAsync()
	{
		return await Context.Select(t => t.Id).ToListAsync();
	}

	public async Task<bool> ExistsAsync(Guid guid)
	{
		return await Context.AnyAsync(x => x.Id == guid);
	}

	public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
	{
		return await Context.AnyAsync(expression);
	}

	/// <summary>
	/// Returns entity with mathcing ID or fails if multiple matches.
	/// </summary>
	/// <param name="guid">Returns entity of matched id</param>
	/// <returns></returns>
	public async Task<T> GetByIdAsync(Guid guid)
	{
		return await Context.SingleAsync(x => x.Id == guid);
	}

	/// <summary>
	/// Updates an entity with parsed object
	/// </summary>
	/// <param name="entity">Entity which has been changed and will be updated to</param>
	/// <returns>Updated entity</returns>
	/// <exception cref="NullReferenceException"></exception>
	public async Task<T> UpdateAsync(T entity)
	{
		if (entity != null)
		{
			var existing = await Context.SingleAsync(t => t.Id == entity.Id);

			if (existing != null)
			{
				Context.Entry(existing).CurrentValues.SetValues(entity);
				await _EFCoreContext.SaveChangesAsync();
				return entity;
			}
		}

		throw new NullReferenceException("You cannot update an entity by parsing null");
	}

	public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> expression)
	{
		return await Context.Where(expression).ToListAsync();
	}

	public async Task<T> FindSingleAsync(Expression<Func<T, bool>> expression)
	{
		return await Context.SingleAsync(expression);
	}

	public async Task<T> FindFirstAsync(Expression<Func<T, bool>> expression)
	{
		return await Context.FirstAsync(expression);
	}
}
