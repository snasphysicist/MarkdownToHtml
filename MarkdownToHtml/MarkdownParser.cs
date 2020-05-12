
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

        // Parse a plain paragraph
        private bool ParseParagraph(
            ArraySegment<string> lines,
            LinkedList<IHtmlable> content
        ) {
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                if (endsWithAtLeastTwoSpaces(line))
                {
                    string shortened = StripTrailingWhitespace(line);
                    foreach (IHtmlable entry in ParseInnerText(shortened))
                    {
                        innerContent.AddLast(entry);
                    }
                    innerContent.AddLast(
                        new MarkdownLinebreak()
                    );
                } else {
                    foreach (IHtmlable entry in ParseInnerText(line))
                    {
                        innerContent.AddLast(entry);
                    }
                    /*
                     * If this is not the last line,
                     * it doesn't end in a manual linebreak
                     * and the user hasn't added a space themselves
                     * we need to add a space at the end
                     */
                    if (
                        (i != (lines.Count - 1))
                        && (line.Length > 0)
                        && (line[^1] != ' ')
                    ) {
                        innerContent.AddLast(
                            new MarkdownText(" ")
                        );
                    }
                }
            }
            MarkdownParagraph paragraph = new MarkdownParagraph(
                LinkedListToArray(innerContent)
            );
            content.AddLast(
                paragraph
            );
            return true;
        }

        private bool endsWithAtLeastTwoSpaces (
            string line
        ) {
            if (line.Length > 1)
            {
                return line.Substring(
                    line.Length - 2,
                    2
                ) == "  ";
            } else
            {
                return false;
            }
        }

        private string StripTrailingCharacter(
            string line,
            char character
        ) {
            while (
                (line.Length > 0)
                && (line[^1] == character)
            ) {
                line = line.Substring(
                    0,
                    line.Length - 1 
                );
            }
            return line;
        }

        private string StripTrailingWhitespace(
            string line
        ) {
            return StripTrailingCharacter(
                line,
                ' '
            );
        }

        private string StripTrailingHashes(
            string line
        ) {
            return StripTrailingCharacter(
                line,
                '#'
            );
        }

        private string StripLeadingWhitespace(
            string line
        ) {
            while (
                (line.Length > 0)
                && (line[0] == ' ')
            ) {
                line = line.Substring(1);
            }
            return line;
        }

        // Parse a single line heading
        private bool ParseSingleLineHeading(
            string line,
            LinkedList<IHtmlable> content
        ) {
            // Work out heading level (cannot exceed 6)
            int level = 0;
            while(
                level < (line.Length)
                && (line[level] == '#')
                && (level < 6)
            ) {
                level++;
            }
            // Remove the leading hashes (up to 6)
            line = line.Substring(level);
            // Remove any leading whitespace
            line = StripLeadingWhitespace(line);
            // Remove any trailing hashes
            line = StripTrailingHashes(line);
            // Remove any trailing whitespace
            line = StripTrailingWhitespace(line);
            content.AddLast(
                new MarkdownHeading(
                    level,
                    ParseInnerText(line)
                )
            );
            return true;
        }

        // Parse a line thought to contain a horizontal rule
        bool ParseHorizontalRule(
            string line,
            LinkedList<IHtmlable> content
        ) {
            // The first character must be present
            if (line.Length == 0)
            {
                return false;
            }
            char used = line[0];
            // The first character must be of the correct type
            if (
                (used != '-')
                && (used != '_')
                && (used != '*')
            ) {
                return false;
            }
            int index = 0;
            while (
                (index < line.Length)
                && (
                    line[index] == used
                )
            ) {
                index++;
            }
            /* 
             * There must be at least three characters
             * and they must be of the same type
             */
            if (
                (index < 3)
                || (index != line.Length)
            ) {
                return false;
            }
            content.AddLast(
                new MarkdownHorizontalRule()
            );
            return true;
        }

        // Parse a quote
        private bool ParseQuote (
            ArraySegment<string> lines,
            LinkedList<IHtmlable> content
        ) {
            string[] truncatedLines = new string[lines.Count];
            // Remove quote arrows and spaces, if needed
            for (int i = 0; i < lines.Count; i++)
            {
                string truncated = lines[i];
                if (lines[i].StartsWith(">"))
                {
                    truncated = truncated.Substring(1);
                    int spaces = 0;
                    // Count spaces
                    while(
                        (spaces < truncated.Length)
                        && (truncated[spaces] == ' ')
                    ) {
                        spaces++;
                    }
                    // If there are fewer than 5 spaces, remove all
                    if (spaces < 5)
                    {
                        truncatedLines[i] = truncated.Substring(spaces);
                    } else {
                        // More than five, just remove one space
                        truncatedLines[i] = truncated.Substring(1);
                    }
                } else {
                    truncatedLines[i] = lines[i];
                }
            }
            /* 
             * The truncated lines should be parsed as any other line group
             * and wrapped in a blockquote element
             */
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            bool lineGroupSuccess = ParseLineGroup(
                new ArraySegment<string>(
                    truncatedLines,
                    0,
                    truncatedLines.Length
                ),
                innerContent
            );
            MarkdownQuote quoteElement = new MarkdownQuote(
                LinkedListToArray(innerContent)
            );
            content.AddLast(
                quoteElement
            );
            return lineGroupSuccess;
        }

        // Parse a quote
        private bool ParseCodeBlock (
            ArraySegment<string> lines,
            LinkedList<IHtmlable> content
        ) {
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            for (int i = 1; i < lines.Count - 1; i++)
            {
                innerContent.AddLast(
                    new MarkdownText(
                        lines[i]
                    )
                );
            }
            MarkdownParagraph blockCodeElement = new MarkdownParagraph(
                new IHtmlable[] {
                    new MarkdownCodeBlock(
                        LinkedListToArray(innerContent)
                    )
                }
            );
            content.AddLast(
                blockCodeElement
            );
            return true;
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
