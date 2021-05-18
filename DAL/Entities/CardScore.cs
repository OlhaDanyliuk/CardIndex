using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class CardScore
    {

        public CardScore() { }
        public CardScore(Card card)
        {
            Card = card;
            Assessment = new HashSet<Assessment>();
        }

        public long Id { get; set; }
        public long CardId { get; set; }
        
        public virtual Card Card { get; set; }
        public virtual ICollection<Assessment> Assessment { get; set; } 
    }
}
