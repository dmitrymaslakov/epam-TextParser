using System.Collections.Generic;
using TextParser.DAL.Abstract;

namespace TextParser.DAL.Entities
{
    public class Punctuation : BasicSentenceItem
    {
        public bool IsSentenceSeparator { get; set; } = false;

        public Punctuation(string value, IEnumerable<int> positions, ItemTypes type) : base(value, positions, type)
        {
            
        }
    }
}
