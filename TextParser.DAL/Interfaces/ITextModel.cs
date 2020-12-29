using System.Collections.Generic;
using TextParser.DAL.Entities;

namespace TextParser.DAL.Interfaces
{
    public interface ITextModel
    {
        List<ISentenceItem> TextItemsStore { get; set; }

        void PrintSentencesInAscendingOrder();
        void PrintWordsIn(SentenceTypes sentenceType, int wordLength);
        void DeleteWordBy(int wordLength, bool isConsonantLetter);
        void PrintToFile(string path);
        void ReplaceWordsInSentences(int[] sentencesPosition, int replacableWordLenght, string replacement);
    }
}
