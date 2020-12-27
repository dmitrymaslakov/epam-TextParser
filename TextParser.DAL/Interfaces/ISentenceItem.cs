using System;
using System.Collections.Generic;
using System.Text;
using TextParser.DAL.Entities;

namespace TextParser.DAL.Interfaces
{
    public interface ISentenceItem
    {
        string Value { get; }
        IEnumerable<int> Positions { get; }
        ItemTypes Type { get; }

        void AddPosition(int position);
        void DisplaceItems(int fromPosition, int steps);
        void DeletePositions(IEnumerable<int> positions);
    }
}
