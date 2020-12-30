using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TextParser.DAL.Entities;
using TextParser.DAL.Interfaces;

namespace TextParser.DAL.Abstract
{
    public abstract class BasicSentenceItem : ISentenceItem, IEnumerable<ISymbol>
    {
        private string _value;

        public virtual string Value
        {
            get => _value;
            set
            {
                _value = value;

                Symbols = Value.Select(ch => new Symbol(ch));

                Length = Value.Length;
            }
        }
        public virtual int Length { get; protected set; }
        public virtual IEnumerable<ISymbol> Symbols { get; protected set; }
        public virtual IEnumerable<int> Positions { get; set; }
        public ItemTypes Type { get; }

        protected BasicSentenceItem(IEnumerable<int> positions, ItemTypes type)
        {
            Positions = positions;
            Type = type;
        }

        protected BasicSentenceItem(string value, IEnumerable<int> positions, ItemTypes type)
        {
            Value = value;

            Positions = positions;

            Symbols = Value.Select(ch => new Symbol(ch));
            
            Length = Value.Length;

            Type = type;
        }

        public virtual void AddPositions(IEnumerable<int> positions)
        {

            Positions = Positions.Concat(positions);
        }

        public virtual void DisplaceItems(int fromPosition, int steps)
        {
            Positions = Positions
                .Select(p => p.Equals(fromPosition) ? p - steps : p)
                .ToList();
        }

        public virtual void DeletePositions(IEnumerable<int> positions)
        {
            Positions = Positions.Where(p => !positions.Any(innerP => innerP.Equals(p)));
        }

        public IEnumerator<ISymbol> GetEnumerator()
        {
            return Symbols.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Symbols.GetEnumerator();
        }
    }
}
