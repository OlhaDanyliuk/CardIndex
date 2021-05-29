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
            _dbContext.Cards.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        public void Add(Card entity)
        {
            _dbContext.Cards.Add(entity);
            _dbContext.SaveChanges();
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
            return _dbContext.Cards.Include(x=>x.Category).Include(x=>x.Assessment).AsQueryable();
        }

        public IQueryable<Card> GetAllWithDetails()
        {
            return _dbContext.Cards.Include(x => x.Assessment).Include(x => x.Category).AsQueryable();
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
            var el = _dbContext.Cards.FirstOrDefault(x=>x.Id==entity.Id);
            foreach (var proptr in entity.GetType().GetProperties())
            {
                var value = proptr.GetValue(entity, null);
                if (proptr.Name == "Category")
                {
                    proptr.SetValue(el, _dbContext.Categories.Find(entity.Category.Id));
                }
                else if (proptr.Name != "Id" && value!= proptr.GetValue(el, null))
                    proptr.SetValue(el, value, null);

            }
            _dbContext.Entry(el).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
