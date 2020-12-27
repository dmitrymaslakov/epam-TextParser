using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextParser.DAL.Abstract;
using TextParser.DAL.Interfaces;

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
