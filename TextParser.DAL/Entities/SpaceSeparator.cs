using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextParser.DAL.Abstract;
using TextParser.DAL.Interfaces;

namespace TextParser.DAL.Entities
{
    public class SpaceSeparator : BasicSentenceItem
    {
        public override IEnumerable<ISymbol> Symbols { get; set; }
        public override string Value => " ";

        public SpaceSeparator(IEnumerable<int> positions, ItemTypes type) : base(positions, type)
        {
            Symbols = Value.Select(ch => new Symbol(ch));
        }
    }
}
