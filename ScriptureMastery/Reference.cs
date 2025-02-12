// Date/Time: 2025-02-04 12:10:00
// The Reference class: Because even scriptures need proper citations!
// This class supports both single verses (e.g., "John 3:16") and verse ranges (e.g., "Proverbs 3:5-6").
using System;

namespace ScriptureMastery
{
    public class Reference
    {
        private string _book;
        private int _chapter;
        private int _startVerse;
        private int? _endVerse; // Nullable: if null, then it's a single verse

        // Constructor for a single verse (e.g., "John 3:16")
        public Reference(string book, int chapter, int verse)
        {
            _book = book;
            _chapter = chapter;
            _startVerse = verse;
            _endVerse = null;
        }

        // Constructor for a verse range (e.g., "Proverbs 3:5-6")
        public Reference(string book, int chapter, int startVerse, int endVerse)
        {
            _book = book;
            _chapter = chapter;
            _startVerse = startVerse;
            _endVerse = endVerse;
        }

        // Returns the string representation of the reference.
        public override string ToString()
        {
            if (_endVerse.HasValue)
            {
                return $"{_book} {_chapter}:{_startVerse}-{_endVerse.Value}";
            }
            else
            {
                return $"{_book} {_chapter}:{_startVerse}";
            }
        }
    }
}
