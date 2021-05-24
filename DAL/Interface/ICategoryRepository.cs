using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICategoryRepository:IRepository<Category>
    {
        IQueryable<Category> GetAllWithDetails();
        IQueryable<Card> GetCardByCategoryId(long id);
    }
}
