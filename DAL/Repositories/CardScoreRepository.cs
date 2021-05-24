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
            throw new NotImplementedException();
        }

        public void Delete(CardScore entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CardScore> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<CardScore> GetAllWithDetails()
        {
            throw new NotImplementedException();
        }

        public Task<CardScore> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<CardScore> GetByIdWithDetailsAsync(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(CardScore entity)
        {
            throw new NotImplementedException();
        }
    }
}
