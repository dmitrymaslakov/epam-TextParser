using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextParser.DAL.Abstract;
using TextParser.DAL.Interfaces;

namespace TextParser.DAL.Entities
{
    public class Word : BasicSentenceItem
    {
        /*public override string Value 
        { 
            get => base.Value; 
            set 
            { 
                base.Value = value;

                Symbols = Value.Select(ch => new Symbol(ch));

                IsCapitalized = char.IsUpper(Symbols.FirstOrDefault().Value);

                Length = Value.Length;
            }
        }*/


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
