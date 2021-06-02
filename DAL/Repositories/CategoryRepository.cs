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
            var el = _dbContext.Categories.FirstOrDefault(x => x.Id == entity.Id);
            foreach (var proptr in entity.GetType().GetProperties())
            {
                var value = proptr.GetValue(entity, null);
                if (proptr.Name != "Id" && proptr.Name != "Cards" && value != proptr.GetValue(el, null))
                    proptr.SetValue(el, value, null);

            }
            _dbContext.Entry(el).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
