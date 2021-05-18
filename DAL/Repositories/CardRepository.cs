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
            throw new NotImplementedException();
        }

        public void Delete(Card entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Card> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Card> GetAllWithDetails()
        {
            throw new NotImplementedException();
        }

        public Task<Card> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CardScore> GetByIdWithDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Card entity)
        {
            throw new NotImplementedException();
        }
    }
}
