
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownParser
    {

        // Regex matches
        Regex regexHorizontalLine = new Regex(
            @"^\*\*\*|^---|^___"
        );
        
        Regex regexOrderedListItem = new Regex(
            @"^\s*\d+\."        // Format 1. (ordered list)
        );

        Regex regexBacktickBlockCode = new Regex(
            @"^```"
        );

        Regex regexUnorderedListItem = new Regex(
            @"^\s*\+\s+|"       // Format +  (unordered list)
            + @"^\s*\*\s+|"     // Format *  (unordered list)
            + @"^\s*-\s+"       // Format -  (unordered list)
        );

        public bool Success
        { get; private set; }

        public IHtmlable[] Content
        { get; private set; }

        // TODO replace with MarkdownDocument object
        public string ToHtml()
        {
            string html = "";
            foreach (IHtmlable entry in Content)
            {
                html += entry.ToHtml();
            }
            return html;
        }

        public MarkdownParser(
            string[] lines
        ) {
            // Assume success
            Success = true;
            // Store parsed content as we go
            LinkedList<IHtmlable> content = new LinkedList<IHtmlable>();
            int currentIndex = 0;
            ArraySegment<string> lineGroup;
            while (currentIndex < lines.Length) {
                lineGroup = NextLineGroup(
                    lines,
                    currentIndex
                );
                bool lineGroupSuccess = ParseLineGroup(
                    lineGroup,
                    content
                );
                Success = Success && lineGroupSuccess; 
                currentIndex += lineGroup.Count + 1;
            }
            Content = new IHtmlable[content.Count];
            content.CopyTo(Content, 0);
        }

        /* 
         * Given a start index, return the
         * line group that starts at that index
         */ 
        private ArraySegment<string> NextLineGroup(
            string[] lines,
            int startIndex
        ) {
            int endIndex = -1;
            // Block code requires slightly different handling
            if (regexBacktickBlockCode.Match(lines[startIndex]).Success)
            {
                endIndex = FindBacktickCodeSectionEnd(
                    lines,
                    startIndex + 1
                ) + 1;
            }
            /* 
             * If the start line isn't a code block or end couldn't be found
             * then try to parse it as some other type
             */
            if (endIndex <= 0) {
                endIndex = FindNextNonMatchingLine(
                    lines,
                    startIndex
                );
            }
            int elements = endIndex - startIndex;
            return new ArraySegment<string>(
                lines, 
                startIndex, 
                elements
            );
        }

        /*
         * Given an array of lines of text and
         * the index of the line at which to start
         * find the first line after it which is
         * either whitespace 
         * or represents a different element type
         */
        private int FindNextNonMatchingLine(
            string[] lines,
            int startIndex
        ) {
            int nextIndex = startIndex + 1;
            while (
                (nextIndex < lines.Length)
                && ContainsOnlyWhitespace(
                    lines[nextIndex]
                )
            ) {
                nextIndex++;
            }
            // Return if we reached the end of the array
            if (nextIndex == lines.Length)
            {
                return nextIndex;
            }
            // Reached a non whitespace line - need to check line types
            MarkdownElementType firstLineType = IdentifyLineType(
                lines[startIndex]
            );
            MarkdownElementType lastLineType = IdentifyLineType(
                lines[nextIndex]
            );
            /* 
             * If this is the actual next line
             * or if types are the same UNLESS both paragraphs
             * then keep adding lines to this group
             * else return
             */
            if (
                (nextIndex != (startIndex + 1))
                && (
                    (firstLineType != lastLineType)
                    || (firstLineType == MarkdownElementType.Paragraph)
                )
            ) {
                return nextIndex - 1;
            } else {
                return FindNextNonMatchingLine(
                    lines,
                    nextIndex
                );
            }
        }

        // Find the first line which starts with three backticks
        private int FindBacktickCodeSectionEnd(
            string[] lines,
            int startIndex
        ) {
            int finalLine = startIndex;
            while (
                (finalLine < lines.Length)
                && (
                    !regexBacktickBlockCode.Match(
                        lines[finalLine]
                    ).Success
                )
            ) {
                finalLine++;
            }
            if (finalLine < lines.Length)
            {
                // Found line with backticks successfully
                return finalLine;
            } else {
                // Did not find line, return negative number
                return -1;
            }
        }

        // Check whether a line contains only whitespace (or is empty)
        private bool ContainsOnlyWhitespace(
            string line
        ) {
            return line.Replace(
                " ",
                ""
            ).Length == 0;
        }

        private MarkdownElementType IdentifyLineType(
            string line
        ) {
            if (line.StartsWith(">"))
            {
                return MarkdownElementType.Quote;
            } else if (regexOrderedListItem.Match(line).Success)
            {
                return MarkdownElementType.OrderedList;
            } else if (regexUnorderedListItem.Match(line).Success)
            {
                return MarkdownElementType.UnorderedList;
            } else 
            {
                return MarkdownElementType.Paragraph;
            }
        }

        private bool ParseLineGroup(
            ArraySegment<string> lines,
            LinkedList<IHtmlable> content
        ) {
            string firstLine = lines[0];
            string lastLine = lines[^1];
            if (
                firstLine.StartsWith("#")
            ) {
                // Single line heading
                return ParseSingleLineHeading(
                    lines[0],
                    content
                );
            } else if (regexHorizontalLine.Match(firstLine).Success) {
                return ParseHorizontalRule(
                    lines[0],
                    content
                );
            } else if (
                firstLine.StartsWith(">")
            ) {
                return ParseQuote(
                    lines,
                    content
                );
            } else if (
                regexBacktickBlockCode.Match(firstLine).Success
                && regexBacktickBlockCode.Match(lastLine).Success
            ) {
                return ParseCodeBlock(
                    lines,
                    content
                );
            } else {
                return ParseParagraph(
                    lines,
                    content
                );
            }
        }

        // Given a single line of text, parse this, including special (emph, etc...) sections
        public static IHtmlable[] ParseInnerText(
            string line
        ) {
            // Store parsed content as we go
            LinkedList<IHtmlable> content = new LinkedList<IHtmlable>();
            // Until the whole string has been consumed
            while (line.Length > 0)
            {
                ParseResult result;
                if (MarkdownStrong.CanParseFrom(line))
                {
                    result = MarkdownStrong.ParseFrom(line);
                } else if (MarkdownStrikethrough.CanParseFrom(line))
                {
                    result = MarkdownStrikethrough.ParseFrom(line);
                } else if (MarkdownEmphasis.CanParseFrom(line))
                {
                    result = MarkdownEmphasis.ParseFrom(line);
                } else if (MarkdownCodeInline.CanParseFrom(line))
                {
                    result = MarkdownCodeInline.ParseFrom(line);
                } else {
                    result = MarkdownText.ParseFrom(
                        line,
                        false
                    );
                }
                /*
                 * If no parsing method suceeded
                 * for once character to be parsed as text
                 */
                if (!result.Success)
                {
                    result = MarkdownText.ParseFrom(
                        line,
                        true
                    );
                }
                // Extract parsed content
                foreach (IHtmlable entry in result.GetContent())
                {
                    content.AddLast(entry);
                }
                // Update text to be parsed
                line = result.Line;
            }
            IHtmlable[] contentArray = new IHtmlable[content.Count];
            content.CopyTo(
                contentArray, 
                0
            );
            return contentArray;
        }

        private T[] LinkedListToArray<T>(
            LinkedList<T> linkedList
        ) {
            T[] array = new T[linkedList.Count];
            linkedList.CopyTo(array, 0);
            return array;
        }

    }
}
