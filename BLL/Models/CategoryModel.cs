using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class CategoryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<long> CardsId { get; set; }
    }
}
