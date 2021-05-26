using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CardScoreRepository : ICardScoreRepository
    {
        private readonly CardDbContext _dbContext;
        public CardScoreRepository(CardDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task AddAsync(CardScore entity)
        {
            _dbContext.CardScores.Add(entity);
            return _dbContext.SaveChangesAsync();
        }

        public void Delete(CardScore entity)
        {
            _dbContext.CardScores.Remove(entity);
            _dbContext.SaveChanges();
        }

        public Task DeleteByIdAsync(long id)
        {
            _dbContext.CardScores.Remove(_dbContext.CardScores.Find(id));
            return _dbContext.SaveChangesAsync();
        }

        public IQueryable<CardScore> GetAll()
        {
            return _dbContext.CardScores.AsQueryable();
        }

        public Task<CardScore> GetByIdAsync(long id)
        {
            return _dbContext.CardScores.FindAsync(id).AsTask();
        }


        public void Update(CardScore entity)
        {
            _dbContext.CardScores.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
