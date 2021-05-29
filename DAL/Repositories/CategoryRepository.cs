using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CardDbContext _dbContext;
        public CategoryRepository(CardDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task AddAsync(Category entity)
        {
            _dbContext.Categories.Add(entity);
            return _dbContext.SaveChangesAsync();
        }

        public void Delete(Category entity)
        {
            _dbContext.Categories.Remove(entity);
            _dbContext.SaveChanges();
        }

        public Task DeleteByIdAsync(long id)
        {
            _dbContext.Categories.Remove(_dbContext.Categories.Find(id));
            return _dbContext.SaveChangesAsync();
        }

        public IQueryable<Category> GetAll()
        {
            return _dbContext.Categories.Include(x=>x.Cards).AsQueryable();
        }

        public IQueryable<Category> GetAllWithDetails()
        {
            return _dbContext.Categories.Include(x => x.Cards).AsQueryable();
        }

        public Task<Category> GetByIdAsync(long id)
        {
            return _dbContext.Categories.FindAsync(id).AsTask();
        }

        public Task<Category> GetByIdWithDetailsAsync(long id)
        {
            return _dbContext.Categories.Include(x => x.Cards).FirstAsync(x => x.Id == id);
        }
    

        public IQueryable<Card> GetCardByCategoryId(long id)
        {
            return _dbContext.Categories.Include(x => x.Cards).First(x=>x.Id==id).Cards.AsQueryable();
        }

        public void Update(Category entity)
        {
            _dbContext.Categories.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
