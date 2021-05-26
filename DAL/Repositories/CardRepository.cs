using DAL.Entities;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
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
        public async Task AddAsync(Card entity)
        {
            var category = _dbContext.Categories.AsNoTracking().First(x => x.Id == entity.Category.Id);
            entity.Category = category;
            await _dbContext.Cards.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        public void Add(Card entity)
        {
            var category = _dbContext.Categories.AsNoTracking().First(x => x.Id == entity.Category.Id);
            entity.Category = category;
            _dbContext.Cards.Add(new Card { Id=entity.Id, Name=entity.Name, Text=entity.Text, Category=category });

            //_dbContext.Entry(entity).State = EntityState.Added;
            var result =_dbContext.SaveChanges();
            //return result;
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

        public IQueryable<Card> GetAllWithDetails()
        {
            return _dbContext.Cards.Include(x => x.Assessment).AsQueryable();
        }

        public double GetAverageScoreByCardId(long id)
        {
            var cardScores = _dbContext.CardScores.Where(x => x.CardId == id).ToList();
            var averageScore = cardScores.Sum(x => x.Assessment) / cardScores.Count;
            return averageScore;
        }

        public Task<Card> GetByIdAsync(long id)
        {
            return _dbContext.Cards.FindAsync(id).AsTask();
        }

        public Card GetWithDetailsById(long id)
        {
            return _dbContext.Cards.Include(x => x.Assessment).First(x=>x.Id==id);
        }

        public void Update(Card entity)
        {
            _dbContext.Cards.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
