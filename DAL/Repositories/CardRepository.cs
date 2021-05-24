using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly CardDbContext _dbContext;
        public CardRepository(CardDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task AddAsync(Card entity)
        {
            _dbContext.AddAsync(entity);
            return _dbContext.SaveChangesAsync();
        }

        public void Delete(Card entity)
        {
            _dbContext.Cards.Remove(entity);
            _dbContext.SaveChanges(); 
        }

        public Task DeleteByIdAsync(long id)
        {
            _dbContext.Cards.Remove(_dbContext.Cards.Find(id));
            return _dbContext.SaveChangesAsync();
        }

        public IQueryable<Card> GetAll()
        {
            return _dbContext.Cards.AsQueryable();
        }


        public Task<Card> GetByIdAsync(long id)
        {
            return _dbContext.Cards.FindAsync(id).AsTask();
        }


        public void Update(Card entity)
        {
            _dbContext.Cards.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
