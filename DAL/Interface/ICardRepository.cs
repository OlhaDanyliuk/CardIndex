using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICardRepository: IRepository<Card>
    {
        double GetAverageScoreByCardId(long id);
        IQueryable<Card> GetAllWithDetails();
        Card GetWithDetailsById(long id);
        void Add(Card entity);
    }
}
