
using System;

namespace MarkdownToHtml
{
    class ParseInput
    {
        public ReferencedUrl[] Urls
        { get; }

        private string[] lines;

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

    }
}