using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WordsCount
{
    public class TextElement    // data element class in Model, describing single text string 
    {
        [JsonProperty("text")]
        public string Text { get; }     // readonly text property
        public int WordsCount           // readonly total wordscount property
        {
            get { return CountWords(); }
        }
        public int VowelsCount      // readonly vowelscount property
        {
            get { return CountVowels(); }
        }
        public TextElement(string text)    // class constructor, receiving a text as parameter
        {
            Text = text;
        }
        private int CountWords()   // count words in text
        {
            return Text.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        }
        private int CountVowels()  // count vowels in sentence
        {
            string clearText = RemoveDiacritics();   // first convert diacritics letters to normal form
            return Regex.Matches(clearText, @"[aeiouаеёиоуыэюя]", RegexOptions.IgnoreCase).Count;
        }

        private string RemoveDiacritics() //convert letters with diacritic signs to its normal form 
        {
            string normalizedString = Text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char symbol in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(symbol);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(symbol);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
