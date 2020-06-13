
using System;

namespace MarkdownToHtml
{
    public class ParseInput
    {
        public ReferencedUrl[] Urls
        { get; }

        private Line[] lines;

        public Line FirstLine
        {
            get
            {
                return lines[startIndex];
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
            this.lines = new Line[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                this.lines[i] = new Line(
                    lines[i]
                );
            }
            this.startIndex = startIndex;
            this.elements = elements;
        }

        public ParseInput(
            ParseInput toCopy,
            string line
        ) {
            Urls = toCopy.Urls;
            lines = new Line[]
            {
                new Line(
                    line
                )
            };
            startIndex = 0;
            elements = 1;
        }

        public ArraySegment<Line> Lines()
        {
            return new ArraySegment<Line>(
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