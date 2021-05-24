using BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface ICardService : ICrud<CardModel>
    {
        double GetAverageScoreByCardId(long id);
        ICollection<CardModel> GetAllWithDetails();
        CardModel GetWithDetailsById(long id);
    }
}
