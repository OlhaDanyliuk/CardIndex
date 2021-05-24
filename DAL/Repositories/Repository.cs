using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly CardDbContext _context;

        public Repository(CardDbContext context)
        {
            this._context = context;
        }
        public Task AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().AddAsync(entity);
            return _context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChangesAsync();
        }

        public Task DeleteByIdAsync(long id)
        {
            _context.Set<TEntity>().Remove(_context.Set<TEntity>().Find(id));
            return _context.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public Task<TEntity> GetByIdAsync(long id)
        {
            return _context.Set<TEntity>().FindAsync(id).AsTask();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            _context.SaveChangesAsync();
        }
    }
}
