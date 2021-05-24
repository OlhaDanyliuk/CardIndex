using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class CardScore
    {

        public CardScore() { }

        public long Id { get; set; }
        public long CardId { get; set; }
        public long UserId { get; set; }
        public int Assessment { get; set; }
    }
}
