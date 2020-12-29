using System.Collections.Generic;
using System.Linq;
using TextParser.DAL.Abstract;

namespace TextParser.DAL.Entities
{
    public class Word : BasicSentenceItem
    {
        public bool IsCapitalized { get; private set; }
        public int Length { get; private set; }

        public Word(string value, IEnumerable<int> positions, ItemTypes type) : base(value, positions, type)
        {
            IsCapitalized = char.IsUpper(Symbols.FirstOrDefault().Value);

            Length = Value.Length;
        }

        public void WithCapitalLetter()
        {

        }

        public void WithSmallLetter()
        {

        }
    }
}
