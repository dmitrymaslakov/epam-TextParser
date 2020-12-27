using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TextParser.DAL.Entities;
using TextParser.DAL.Interfaces;

namespace TextParser.BBL
{
    public class Parser
    {
        private const string SENTENCE_SPLITER_LINE_END =
            "(\\.+)|(\\?!)|(\\.)|(\\?)|(!)";
        private const string SENTENCE_SPLITER = @"(\.+)\s|(\.)\s|(\.)\s\r|(\.)\s\r\n|(\?!)\s|(\?)\s|(\!)\s";
        private const string REPLACEABLE = @"\s+";
        private const string SEPARATOR_INDICATOR = "{C9605AD0-9431-40B5-A7EB-E8E98007207B}";
        private const string COMMON_SPLITER =
            "(\\.+)|(\\?!)|(\\.)|(,)|(;)|(:)|(-)|(\\?)|(!)|(’)|(\")|(\\()|(\\))|(\\{)|(})|(\\[)|(])|(\\s)";

        public ITextModel Parse(string filePath)
        {
            var textItemsStore = new List<ISentenceItem>();

            using (var reader = File.OpenText(filePath))
            {
                var sentenceItemPosition = 0;


                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var commonSpliterWithIndicator = "";

                    var isSentenceSpliter = Regex.IsMatch(line, SENTENCE_SPLITER);

                    if (isSentenceSpliter)
                    {
                        var m = Regex.Match(line, SENTENCE_SPLITER);

                        line = Regex.Replace(line, $"\\{m.Value}", $"{SEPARATOR_INDICATOR}{m.Value}").Trim();

                        var addedSpliter = $"{SEPARATOR_INDICATOR}\\{m.Value.Trim()}";

                        commonSpliterWithIndicator = $"({addedSpliter})|{COMMON_SPLITER}";
                    }

                    line = Regex.Replace(line, REPLACEABLE, " ").Trim();

                    var sentenceItems = isSentenceSpliter
                        ?
                        Regex.Split(line, commonSpliterWithIndicator)
                        :
                        Regex.Split(line, COMMON_SPLITER);

                    sentenceItems = SpliterAtEndOfLine(sentenceItems);

                    foreach (var item in sentenceItems)
                    {
                        if (string.IsNullOrEmpty(item)) continue;

                        AddPositionOrNewItem(sentenceItemPosition, textItemsStore, item);

                        sentenceItemPosition++;
                    }

                    sentenceItemPosition++;

                    if (textItemsStore.Any(s => Equals(s.Value, Environment.NewLine)))
                    {
                        textItemsStore
                            .SingleOrDefault(s => Equals(s.Value, Environment.NewLine))
                            .AddPosition(sentenceItemPosition);
                    }
                    else
                    {
                        textItemsStore.Add(new NewLineSymbol(new int[] { sentenceItemPosition }, ItemTypes.NewLine));
                    }

                    sentenceItemPosition++;
                }

            }

            return new TextModel(textItemsStore);
        }

        private string[] SpliterAtEndOfLine(string[] splitedLine)
        {
            var lastItem = splitedLine
                .Where(i => !string.IsNullOrEmpty(i))
                .Last()
                ;

            if (Regex.IsMatch(lastItem, SENTENCE_SPLITER_LINE_END))
            { 
                var separatorIndicator = Regex.Replace(lastItem, $"\\{lastItem}", $"{SEPARATOR_INDICATOR}{lastItem}");

                /*var addedSpliter = $"{SEPARATOR_INDICATOR}\\{splitedLine.Last()}";

                commonSpliterWithIndicator = $"({addedSpliter})|{COMMON_SPLITER}";
                
                line = string.Join("", splitedLine.SkipLast(1).ToArray()) + s;*/

                splitedLine = splitedLine
                    .Where(i => !string.IsNullOrEmpty(i))
                    .SkipLast(1)
                    .Append(separatorIndicator)
                    .ToArray();
            }

            return splitedLine;
        }

        private void AddPositionOrNewItem(
            int sentenceItemPosition, List<ISentenceItem> sentenceItemsStore, string item)
        {
            if (item.Contains(SEPARATOR_INDICATOR))
            {
                var itemWithoutIndicator = Regex.Replace(item, SEPARATOR_INDICATOR, "");

                var punctuation = sentenceItemsStore
                    .Where(i => Equals(i.Value, itemWithoutIndicator))
                    .Cast<Punctuation>()
                    .Where(p => p.IsSentenceSeparator == true)
                    .SingleOrDefault();

                if (punctuation != null)
                {
                    punctuation.AddPosition(sentenceItemPosition);
                }
                else
                {
                    AddNewSentenceItem(item, sentenceItemPosition, sentenceItemsStore);
                }
            }
            else
            {
                var existItems = sentenceItemsStore
                    .Where(i => Equals(i.Value, item));

                if (existItems.Count() != 0)
                {
                    var existItem = (existItems.First() as Punctuation) is null ?
                        existItems.Single() :
                        existItems.Single(i => !((Punctuation)i).IsSentenceSeparator);

                    existItem.AddPosition(sentenceItemPosition);
                }
                else
                {
                    AddNewSentenceItem(item, sentenceItemPosition, sentenceItemsStore);
                }

            }
        }

        private void AddNewSentenceItem(
            string newItem, int newItemPosition, List<ISentenceItem> sentenceItemsStore)
        {
            if (char.IsLetter(newItem, 0) || char.IsDigit(newItem, 0))
            {
                sentenceItemsStore.Add(new Word(newItem, new int[] { newItemPosition }, ItemTypes.Word));
            }
            else if (char.IsSeparator(newItem, 0))
            {
                sentenceItemsStore.Add(new SpaceSeparator(new int[] { newItemPosition }, ItemTypes.SpaceSeparator));
            }
            else if (char.IsPunctuation(newItem, 0))
            {
                AddPunctuation(newItem, newItemPosition, sentenceItemsStore);
            }
        }

        private void AddPunctuation(string newItem, int newItemPosition, List<ISentenceItem> sentenceItemsStore)
        {
            if (newItem.Contains(SEPARATOR_INDICATOR))
            {
                newItem = Regex.Replace(newItem, SEPARATOR_INDICATOR, "");

                sentenceItemsStore.Add(new Punctuation(newItem, new int[] { newItemPosition }, ItemTypes.Punctuation) { IsSentenceSeparator = true });
            }
            else
                sentenceItemsStore.Add(new Punctuation(newItem, new int[] { newItemPosition }, ItemTypes.Punctuation));
        }
    }
}
