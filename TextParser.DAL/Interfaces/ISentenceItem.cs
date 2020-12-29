using System.Collections.Generic;
using TextParser.DAL.Entities;

namespace TextParser.DAL.Interfaces
{
    public interface ISentenceItem
    {
        string Value { get; }
        IEnumerable<int> Positions { get; }
        ItemTypes Type { get; }

        void AddPositions(IEnumerable<int> position);
        void DisplaceItems(int fromPosition, int steps);
        void DeletePositions(IEnumerable<int> positions);
    }
}
