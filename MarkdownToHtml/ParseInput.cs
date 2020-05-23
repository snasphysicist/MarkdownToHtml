
using System;

namespace MarkdownToHtml
{
    public class ParseInput
    {
        public ReferencedUrl[] Urls
        { get; }

        private string[] lines;

        public string FirstLine
        {
            get
            {
                return lines[startIndex];
            }
            set
            {
                lines[startIndex] = value;
            }
        }

        private int startIndex;

        private int elements;

        public ParseInput(
            ReferencedUrl[] urls,
            string[] lines,
            int startIndex,
            int elements
        ) {
            Urls = urls;
            this.lines = lines;
            this.startIndex = startIndex;
            this.elements = elements;
        }

        public ParseInput(
            ParseInput toCopy,
            string line
        ) {
            Urls = toCopy.Urls;
            lines = new string[]
            {
                line
            };
            startIndex = 0;
            elements = 1;
        }

        public ArraySegment<string> Lines()
        {
            return new ArraySegment<string>(
                lines,
                startIndex,
                elements
            );
        }

        public ParseInput Slice(
            int startIndex,
            int elements
        ) {
            this.startIndex = startIndex;
            this.elements = elements;
            return this;
        }

        public ParseInput NextLine()
        {
            this.startIndex++;
            this.elements--;
            return this;
        }

        public ParseInput JumpLines(
            int numberOfLines
        ) {
            this.startIndex += numberOfLines;
            this.elements -= numberOfLines;
            return this;
        }

    }
}