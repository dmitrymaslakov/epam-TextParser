using System.Collections.Generic;
using TextParser.DAL.Entities;

namespace TextParser.DAL.Interfaces
{
    public interface ISentence
    {
        List<ISentenceItem> TextItemsStore { get; }
        int FirstItemPosition { get; }
        int LastItemPosition { get; }
        int Position { get; }
        int WordCount { get; }
        SentenceTypes Type { get; }

        void PrintToConsole();
        IEnumerable<Word> GetWordsBy(int length);
        void ReplaceWordsBy(int length, string replacement);
    }
}
