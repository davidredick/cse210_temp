// Date/Time: 2025-02-04 12:11:00
// The Scripture class: Because a scripture is more than just a wall of text—it’s a challenge!
// This class holds the reference and the scripture text as a list of Word objects.
// Modified to preserve carriage returns and newlines.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScriptureMastery
{
    public class Scripture
    {
        private Reference _reference;
        private List<Word> _words;
        private string _originalText; // To allow for a complete reset

        // Constructor using Regex.Split to capture all whitespace (including newlines)
        public Scripture(Reference reference, string text)
        {
            _reference = reference;
            _originalText = text;
            // Splitting the text while capturing whitespace tokens (so that newlines are preserved)
            _words = Regex.Split(text, @"(\s+)")
                          .Where(token => token != "")
                          .Select(token => new Word(token))
                          .ToList();
        }

        // Reset the scripture to its full, unhidden glory.
        public void Reset()
        {
            _words = Regex.Split(_originalText, @"(\s+)")
                          .Where(token => token != "")
                          .Select(token => new Word(token))
                          .ToList();
        }

        // Hides a few random words that aren't already hidden.
        // This method only hides tokens that are not pure whitespace.
        public void HideRandomWords(int count)
        {
            // Get indices of tokens that are not pure whitespace and not hidden.
            var visibleIndices = _words
                .Select((word, index) => new { word, index })
                .Where(item => !string.IsNullOrWhiteSpace(item.word.ToString()) && !item.word.IsHidden())
                .Select(item => item.index)
                .ToList();

            if (visibleIndices.Count == 0)
            {
                return;
            }

            int wordsToHide = Math.Min(count, visibleIndices.Count);
            Random random = new Random();
            visibleIndices = visibleIndices.OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < wordsToHide; i++)
            {
                _words[visibleIndices[i]].Hide();
            }
        }

        // Returns the rendered scripture with the reference on top,
        // preserving the original formatting (including newlines).
        public string GetRenderedText()
        {
            // Concatenate all tokens exactly as they are so that whitespace is preserved.
            string renderedText = string.Concat(_words.Select(word => word.GetRenderedText()));
            return $"{_reference}\n{renderedText}";
        }

        // Returns true if every token that is not pure whitespace is hidden.
        public bool IsCompletelyHidden()
        {
            return _words
                .Where(word => !string.IsNullOrWhiteSpace(word.ToString()))
                .All(word => word.IsHidden());
        }
    }
}
