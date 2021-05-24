using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class CardModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public double? AverageScore { get; set; }
        public ICollection<long> CardScoresIds { get; set; }
        public long CategoryId { get; set; }
        
    }
}
