// Date/Time: 2025-02-04 12:10:30
// The Word class: Because every word deserves its moment of glory... or total obscurity!
// This class manages each word’s text and whether it’s hidden (i.e., playing hide-and-seek).
using System;

namespace ScriptureMastery
{
    public class Word
    {
        private string _text;
        private bool _hidden;

        public Word(string text)
        {
            _text = text;
            _hidden = false;
        }

        // Hide this word—it's time for it to take a little nap.
        public void Hide()
        {
            _hidden = true;
        }

        // Returns true if the word is hidden.
        public bool IsHidden()
        {
            return _hidden;
        }

        // Returns the rendered text: either the original word or underscores of matching length.
        public string GetRenderedText()
        {
            return _hidden && !string.IsNullOrWhiteSpace(_text)
                ? new string('_', _text.Length)
                : _text;
        }

        // For our amusement (and debugging, if you’re into that sort of thing).
        public override string ToString()
        {
            return _text;
        }
    }
}
