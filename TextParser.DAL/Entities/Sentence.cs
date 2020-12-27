using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextParser.DAL.Interfaces;

namespace TextParser.DAL.Entities
{
    public class Sentence : ISentence
    {
        public IEnumerable<ISentenceItem> TextItemsStore { get; set; }
        public int FirstItemPosition { get; set; }
        public int LastItemPosition { get; set; }
        public int Position { get; set; }
        public int WordCount { get; set; }
        public SentenceTypes Type { get; set; }

        public Sentence(
            IEnumerable<ISentenceItem> textItemsStore,
            int firstItemPosition, int lastItemPosition, int position, SentenceTypes type)
        {
            TextItemsStore = textItemsStore;
            FirstItemPosition = firstItemPosition;
            LastItemPosition = lastItemPosition;
            Position = position;
            WordCount = Position == 20 ? 0 : GetWordCount();
            Type = type;
        }

        public void PrintToConsole()
        {
            var textResult = new StringBuilder();

            for (int position = FirstItemPosition; position <= LastItemPosition; position++)
            {
                var item = TextItemsStore.SingleOrDefault(i => i.Positions.Contains(position));

                if (item != null && item.Value.Equals(Environment.NewLine))
                    item = new SpaceSeparator(new int[] { position }, ItemTypes.SpaceSeparator);

                textResult.Append(item?.Value);
            }

            Console.WriteLine(textResult.ToString());
        }

        public IEnumerable<Word> GetWordsBy(int length)
        {
            return Enumerable.Range(FirstItemPosition, LastItemPosition - FirstItemPosition)
                .Select(p => TextItemsStore
                    .SingleOrDefault(i => i.Positions.Contains(p) && i.Type == ItemTypes.Word))
                .Where(i => i != null)
                .Cast<Word>()
                .Where(w => w.Length.Equals(length))
                .Distinct();
            ;
        }

        public void ReplaceWordsBy(int length, string replacement)
        {
            var words = GetWordsBy(length).ToList();

            var positions = Enumerable.Range(FirstItemPosition, LastItemPosition - FirstItemPosition)
                .Where(p => words.Any(i => i.Positions.Contains(p)))
                .ToList()
                ;

            TextItemsStore = TextItemsStore.Append(new Word(replacement, positions, ItemTypes.Word)).ToList();

            foreach (var word in words)
            {
                word.DeletePositions(positions);
            }
        }

        private int GetWordCount()
        {
            return Enumerable.Range(FirstItemPosition, LastItemPosition - FirstItemPosition)
                .Select(p => TextItemsStore
                    .SingleOrDefault(i => i.Positions.Contains(p) && i.Type == ItemTypes.Word))
                .Where(i => i != null)
                .Count()
                ;
        }
    }
}
