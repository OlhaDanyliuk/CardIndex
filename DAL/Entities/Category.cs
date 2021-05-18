using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}
