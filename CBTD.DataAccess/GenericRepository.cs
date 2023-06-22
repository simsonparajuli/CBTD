using CBTD.ApplicationCore.Interfaces;
using CBTD.ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CBTD.DataAccess
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly ApplicationDbContext _dbContext;
		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public void Add(T entity)
		{
			_dbContext.Set<T>().Add(entity);
			_dbContext.SaveChanges();
		}

		public void Delete(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			_dbContext.SaveChanges();
		}

		public void Delete(IEnumerable<T> entities)
		{
			_dbContext.Set<T>().RemoveRange(entities);
			_dbContext.SaveChanges();
		}

		public virtual T Get(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string includes = null)
		{
			if (includes == null)   // we are not joining any objects
			{
				if (!asNotTracking) // set to false, we're not tracking changes
				{
					return _dbContext.Set<T>()
						.Where(predicate)
						.AsNoTracking()
						.FirstOrDefault();
				}

				else // I am tracking changes
				{
					return _dbContext.Set<T>()
						.Where(predicate)
						.FirstOrDefault();
				}
			}

			else // If other objects to include (join)
				 // includes = "Address, Finances,Dependents"
			{
				IQueryable<T> queryable = _dbContext.Set<T>();

				foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}

				if (!asNotTracking) // set to false, we're not tracking changes
				{
					return queryable
						.Where(predicate)
						.AsNoTracking()
						.FirstOrDefault();
				}

				else // I am tracking changes
				{
					return queryable
						.Where(predicate)
						.FirstOrDefault();
				}
			}
		}

		public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string includes = null)
		{
			if (includes == null)   // we are not joining any objects
			{
				if (!asNotTracking) // set to false, we're not tracking changes
				{
					return await _dbContext.Set<T>()
						.Where(predicate)
						.AsNoTracking()
						.FirstOrDefaultAsync();
				}

				else // I am tracking changes
				{
					return await _dbContext.Set<T>()
						.Where(predicate)
						.FirstOrDefaultAsync();
				}
			}

			else // If other objects to include (join)
				 // includes = "Address, Finances,Dependents"
			{
				IQueryable<T> queryable = _dbContext.Set<T>();

				foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}

				if (!asNotTracking) // set to false, we're not tracking changes
				{
					return await queryable
						.Where(predicate)
						.AsNoTracking()
						.FirstOrDefaultAsync();
				}

				else // I am tracking changes
				{
					return await queryable
						.Where(predicate)
						.FirstOrDefaultAsync();
				}
			}
		}

		public virtual T GetById(int? id)
		{
			return _dbContext.Set<T>().Find(id);
		}

		public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null, Expression<Func<T, int>> orderBy = null, string includes = null)
		{
			IQueryable<T> queryable = _dbContext.Set<T>();
			if (predicate != null && includes == null)
			{
				return _dbContext.Set<T>()
					.Where(predicate)
					.AsEnumerable();
			}
			// has optional includes
			else if (includes != null)
			{
				foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}
			}

			if (predicate == null)
			{
				if (orderBy == null)
				{
					return queryable.AsEnumerable();
				}
				else
				{
					return queryable.OrderBy(orderBy).ToList().AsEnumerable();
				}
			}
			else
			{
				if (orderBy == null)
				{

					return queryable.Where(predicate).ToList();

				}
				else
				{
					return queryable.Where(predicate).OrderBy(orderBy).ToList().AsEnumerable();
				}
			}
		}

		public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Expression<Func<T, int>> orderBy = null, string includes = null)
		{
			IQueryable<T> queryable = _dbContext.Set<T>();
			if (predicate != null && includes == null)
			{
				return _dbContext.Set<T>()
					.Where(predicate)
					.AsEnumerable();
			}
			// has optional includes
			else if (includes != null)
			{
				foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}
			}

			if (predicate == null)
			{
				if (orderBy == null)
				{
					return queryable.AsEnumerable();
				}
				else
				{
					return await queryable.OrderBy(orderBy)
						.ToListAsync();

				}
			}
			else
			{
				if (orderBy == null)
				{

					return await queryable.OrderBy(orderBy)
						.ToListAsync();


				}
				else
				{
					return await queryable.Where(predicate)
						.OrderBy(orderBy)
						.ToListAsync();

				}
			}
		}

		public void Update(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
		}

		public int DecrementCount(ShoppingCart shoppingCart, int count)
		{
			shoppingCart.Count -= count;
			return shoppingCart.Count;
		}

		public int IncrementCount(ShoppingCart shoppingCart, int count)
		{
			shoppingCart.Count += count;
			return shoppingCart.Count;
		}
	}
}
