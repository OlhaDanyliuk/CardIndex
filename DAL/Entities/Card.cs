using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Card
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public double? AverageScore { get; set; }

        public virtual ICollection<CardScore> Assessment { get; set; }
        public virtual Category Category { get; set; }
    }
}
