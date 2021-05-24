using BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface ICategoriesService:ICrud<CategoryModel>
    {
        ICollection<CategoryModel> GetAllWithDetails();
        ICollection<CardModel> GetCardByCategoryId(long id);

    }
}
