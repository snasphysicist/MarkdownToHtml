
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

        public Line this[int index]
        {
            get
            {
                return lines[startIndex + index];
            }
        }

        public int Count
        {
            get
            {
                return elements;
            }
        }

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

        private ParseInput(
            ReferencedUrl[] urls,
            Line[] lines,
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
            lines = new Line[]
            {
                new Line(
                    line
                )
            };
            startIndex = 0;
            elements = 1;
        }

        public ParseInput LinesFromStart(
            int numberOfLines
        ) {
            return new ParseInput(
                Urls,
                lines,
                startIndex,
                numberOfLines
            );
        }

        public ParseInput LinesUpTo(
            int endIndex
        ) {
            return new ParseInput(
                Urls,
                lines,
                startIndex,
                endIndex - startIndex
            );
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

        public void NextLine()
        {
            startIndex++;
            elements--;
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