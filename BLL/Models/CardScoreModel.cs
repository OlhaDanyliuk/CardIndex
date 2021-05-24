using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class CardScoreModel
    {
        public long Id { get; set; }
        public long CardId { get; set; }
        public long UserId { get; set; }
        public int Assessment { get; set; }
    }
}
