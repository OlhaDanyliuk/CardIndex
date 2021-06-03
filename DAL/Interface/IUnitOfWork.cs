using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IUnitOfWork
    {
        ICardRepository CardRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICardScoreRepository CardScoreRepository { get; }

        Task<int> SaveAsync();
    }

}
