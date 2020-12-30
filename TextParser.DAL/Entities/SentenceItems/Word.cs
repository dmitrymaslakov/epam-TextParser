using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TextParser.DAL.Abstract;

namespace TextParser.DAL.Entities
{
    public class Word : BasicSentenceItem
    {
        public override string Value 
        { 
            get => base.Value;
            set 
            { 
                base.Value = value;

                IsCapitalized = char.IsUpper(Symbols.FirstOrDefault().Value);
            }
        }
        public bool IsCapitalized { get; private set; }

        public Word(string value, IEnumerable<int> positions, ItemTypes type) : base(value, positions, type)
        {
            IsCapitalized = char.IsUpper(Symbols.FirstOrDefault().Value);
        }

        public void WithCapitalLetter()
        {
            Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Value);
        }

        public void WithSmallLetter()
        {
            Value = Value.ToLower();
        }
    }
}
