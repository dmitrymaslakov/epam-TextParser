using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TextParser.DAL.Interfaces;

namespace TextParser.DAL.Entities
{
    public class TextModel : ITextModel
    {
        private const string CONSONANT_LETTER = @"^[+бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxz]{1,}";

        public IEnumerable<ISentenceItem> TextItemsStore { get; set; }
        public IEnumerable<ISentence> Sentences { get; set; }
        public int Count { get; private set; }

        public TextModel(IEnumerable<ISentenceItem> textItemsStore)
        {
            TextItemsStore = textItemsStore;

            Sentences = GetSentences();

            Count = TextItemsStore.Select(i => i.Positions.Max()).Max();
        }

        public void DeleteWordBy(int wordLength, bool isConsonantLetter)
        {
            var words = Enumerable.Range(0, Count)
                            .Select(p => TextItemsStore
                                .SingleOrDefault(i => i.Positions.Contains(p) && i.Type == ItemTypes.Word))
                            .Where(i => i != null)
                            .Cast<Word>()
                            .Where(w => w.Length.Equals(wordLength))
                            .Distinct();


            if (isConsonantLetter)
            {
                words = words.Where(w => Regex.IsMatch(w.Value, CONSONANT_LETTER, RegexOptions.IgnoreCase));
            }

            foreach (var item in words)
            {
                Console.WriteLine(item.Value);
            }

            var positions = words
                .SelectMany(w => w.Positions)
                .OrderBy(p => p)
                .ToList()
                ;

            foreach (var word in words)
            {
                TextItemsStore = TextItemsStore
                    .Where(i => !i.Equals(word));
            }

            for (int i = 0, temp = 0; i < positions.Count; i++)
            {
                var previous = temp;
                var current = positions[i];
                if (i != 0)
                {
                    for (int position = ++previous; position < current; position++)
                    {
                        TextItemsStore
                                    .SingleOrDefault(i => i.Positions.Contains(position))
                                    ?.DisplaceItems(position, i);
                    }
                }

                temp = current;
            }
            Sentences = GetSentences();

            Count = TextItemsStore.Select(i => i.Positions.Max()).Max();
        }

        public void PrintSentencesInAscendingOrder()
        {
            var sentences = Sentences.OrderBy(s => s.WordCount).ToList();

            foreach (var sentence in sentences)
            {
                sentence.PrintToConsole();

                Console.WriteLine(new string('*', 50));
            }
        }

        public void PrintWordsIn(SentenceTypes sentenceType, int wordLength)
        {
            var sentences = Sentences.Where(s => s.Type == sentenceType);

            foreach (var sentence in sentences)
            {
                Console.WriteLine($"В {sentence.Position} предложении находятся следующие слова заданной длины: ");
                var words = sentence
                    .GetWordsBy(wordLength)
                    .Select(w => w.Value.ToLower())
                    .Distinct()
                    ;

                foreach (var word in words)
                {
                    Console.Write($"{word}, ");
                }
                Console.WriteLine(Environment.NewLine);
            }
        }

        public void PrintToFile(string path)
        {
            var textResult = new StringBuilder();

            for (int position = 0; position < Count; position++)
            {
                var item = TextItemsStore.SingleOrDefault(i => i.Positions.Contains(position));

                textResult.Append(item?.Value);
            }

            File.WriteAllText(path, textResult.ToString());
        }

        public void ReplaceWordsInSentences(int[] sentencesPosition, int replacableWordLenght, string replacement)
        {
            var sentences = Sentences.Where(s => sentencesPosition.Any(p => p.Equals(s.Position))).ToList();

            foreach (var sentence in sentences)
            {
                sentence.ReplaceWordsBy(replacableWordLenght, replacement);
            }
        }

        private SentenceTypes DefineSentenceType(int separatorPosition)
        {
            var separatorValue = TextItemsStore
                .SingleOrDefault(i => i.Positions.Contains(separatorPosition))
                .Value
                ;

            switch (separatorValue)
            {
                case "?!":
                    return SentenceTypes.InterrogativeExclamatory;
                case "?":
                    return SentenceTypes.Interrogative;
                case "!":
                    return SentenceTypes.Exlamatory;
                default:
                    return SentenceTypes.Declarative;
            }
        }

        private IEnumerable<ISentence> GetSentences()
        {
            var offset = 0;
            var count = 1;

            var sentences = TextItemsStore
                .OfType<Punctuation>()
                .Where(p => p != null && p.IsSentenceSeparator == true)
                .SelectMany(p => p.Positions)
                .OrderBy(p => p)
                .ToList()
                .Select(p =>
                {
                    var type = DefineSentenceType(p);
                    var firstItemPosition = offset;
                    var lastItemPosition = p;
                    offset = lastItemPosition + 2;
                    return new Sentence(TextItemsStore, firstItemPosition, lastItemPosition, count++, type);
                })
                ;

            return sentences;
        }

    }
}
