using System;
using System.IO;
using TextParser.BBL;
using TextParser.DAL.Entities;
using System.Configuration;

namespace TextParser.PL
{
    class Program
    {
        static void Main(string[] args)
        {
            var bookPath = ConfigurationManager.AppSettings.Get("book");

            var parsedBookPath = ConfigurationManager.AppSettings.Get("parsedBook");

            var parser = new Parser();

            var text = parser.Parse(bookPath);

            //text.ReplaceWordsInSentences(new int[] { 2 }, 3, "Maslakov"); - не работает!

            //text.DeleteWordBy(3, true);

            text.PrintToFile(parsedBookPath);

            //text.PrintWordsIn(SentenceTypes.Interrogative, 3);

            //text.PrintSentencesInAscendingOrder();
        }
    }
}
