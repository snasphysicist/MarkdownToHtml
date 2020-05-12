
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
        IHtmlable[] ParseInnerText(
            string line
        ) {
            // Store parsed content as we go
            LinkedList<IHtmlable> content = new LinkedList<IHtmlable>();
            // Until the whole string has been consumed
            while (line.Length > 0)
            {
                string initialLine = line;
                if (line.StartsWith("**")) {
                    line = ParseStarStrongSection(
                        line, 
                        content
                    );
                } else if (line.StartsWith("__")) {
                    line = ParseUnderscoreStrongSection(
                        line,
                        content
                    );
                } else if (line.StartsWith("~~")) {
                    line = ParseStrikethroughSection(
                        line,
                        content
                    );
                } else if (line.StartsWith("*"))
                {
                    line = ParseStarEmphasisSection(
                        line,
                        content
                    );
                } else if (line.StartsWith("_")) {
                    line = ParseUnderscoreEmphasisSection(
                        line,
                        content
                    );
                } else if (line.StartsWith("`")) 
                {
                    line = ParseInlineCodeSection(
                        line,
                        content
                    );
                } else {
                    line = ParsePlainTextSection(
                        line,
                        content
                    );
                }
                /*
                 * If there is content left but it cannot be parsed
                 * then fail
                 */
                if (
                    (line.Length > 0)
                    && (initialLine == line)
                ) {
                    Success = false;
                    return new IHtmlable[]{};
                }
            }
            // Plain text case, not yet dealing with strong/emph/etc
            IHtmlable[] contentArray = new IHtmlable[content.Count];
            content.CopyTo(
                contentArray, 
                0
            );
            return contentArray;
        }

        // Given a text snippet, parse a plain text section from its start
        private string ParsePlainTextSection(
            string line,
            LinkedList<IHtmlable> content
        ) {
            int indexEmphasisSectionStart = FindUnescapedSpecial(
                line
            );
            // If there is an emphasis section
            if (indexEmphasisSectionStart != line.Length)
            {
                // Add as plain text only the content up to where it starts
                content.AddLast(
                    new MarkdownText(
                        line.Substring(0, indexEmphasisSectionStart)
                    )
                );
                // Return everything after where it starts
                return line.Substring(indexEmphasisSectionStart);
            } else {
                // If there are no special sections, everything is plain text
                content.AddLast(
                    new MarkdownText(
                        line
                    )
                );
                return "";
            }
        }

        /* 
         * Finds the index of the first unescaped star in the provided string
         * Returns the string string length if none can be found
         */
        private int FindUnescapedSpecial(
            string line
        ) {
            Char[] specialCharacters = new Char[] {
                '*',
                '_',
                '~',
                '`'
            };
            int j = 0;
            while (
                (j < line.Length)
                && !(
                    IsInArray(
                        line[j],
                        specialCharacters
                    )
                    && (line[j-1] != '\\')
                )
            ) {
                j++;
            }
            return j;
        }

        // Given a text snippet starting with a star, parse the emphasis section at its start
        private string ParseStarEmphasisSection(
            string line,
            LinkedList<IHtmlable> content
        ) {
            return ParseEmphasisSection(
                line,
                '*',
                content
            );
        }

        // Given a text snippet starting with a star, parse the emphasis section at its start
        private string ParseUnderscoreEmphasisSection(
            string line,
            LinkedList<IHtmlable> content
        ) {
            return ParseEmphasisSection(
                line,
                '_',
                content
            );
        }

        // Shared code for parsing emphasis sections
        private string ParseEmphasisSection(
            string line,
            char delimiter,
            LinkedList<IHtmlable> content
        ) {
            int j = 1;
            // Find closing star
            while (
                (j < line.Length)
                && !(
                    (line[j] == delimiter)
                    && (line[j-1] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                // If we cannot, then return line as is
                return line;
            }
            // Parse everything inside the stars
            MarkdownEmphasis element = new MarkdownEmphasis(
                ParseInnerText(
                    line.Substring(1, j - 1)
                )
            );
            // Add the new emphasis element to the content
            content.AddLast(
                element
            );
            // Return the line string minus the content we parsed
            return line.Substring(j + 1);
        }

        /* 
         * Given a text snippet starting with two stars
         * parse the emphasis section at its start
         */
        private string ParseStarStrongSection(
            string line,
            LinkedList<IHtmlable> content
        ) {
            return ParseStrongSection(
                line,
                "**",
                content
            );
        }

        /* 
         * Given a text snippet starting with two underscores
         * parse the emphasis section at its start
         */
        private string ParseUnderscoreStrongSection(
            string line,
            LinkedList<IHtmlable> content
        ) {
            return ParseStrongSection(
                line,
                "__",
                content
            );
        }

        // Shared code for parsing strong sections
        private string ParseStrongSection(
            string line,
            string delimiter,
            LinkedList<IHtmlable> content
        ) {
            int j = 2;
            // Find closing star
            while (
                (j < line.Length)
                && !(
                    (line.Substring(j-1, 2) == delimiter)
                    && (line[j-2] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                // If we cannot, then return line as is
                return line;
            }
            // Parse everything inside the stars
            MarkdownStrong element = new MarkdownStrong(
                ParseInnerText(
                    line.Substring(2, j - 3)
                )
            );
            // Add the new emphasis element to the content
            content.AddLast(
                element
            );
            // Return the line string minus the content we parsed
            return line.Substring(j + 1);
        }

        private string ParseStrikethroughSection(
            string line,
            LinkedList<IHtmlable> content
        ) {
            int j = 2;
            // Find closing tildes
            while (
                (j < line.Length)
                && !(
                    (line.Substring(j-1, 2) == "~~")
                    && (line[j-2] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                // If we cannot, then return line as is
                return line;
            }
            // Parse everything inside the stars
            MarkdownStrikethrough element = new MarkdownStrikethrough(
                ParseInnerText(
                    line.Substring(2, j - 3)
                )
            );
            // Add the new element to the content
            content.AddLast(
                element
            );
            // Return the line string minus the content we parsed
            return line.Substring(j + 1);
        }

        // Shared code for parsing emphasis sections
        private string ParseInlineCodeSection(
            string line,
            LinkedList<IHtmlable> content
        ) {
            int j = 1;
            // Find closing star
            while (
                (j < line.Length)
                && !(
                    (line[j] == '`')
                    && (line[j-1] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                // If we cannot, then return line as is
                return line;
            }
            // Parse everything inside the stars
            MarkdownCodeInline element = new MarkdownCodeInline(
                ParseInnerText(
                    line.Substring(1, j - 1)
                )
            );
            // Add the new emphasis element to the content
            content.AddLast(
                element
            );
            // Return the line string minus the content we parsed
            return line.Substring(j + 1);
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

        // Check whether a value is in an array
        private bool IsInArray<T>(
            T value,
            T[] array
        ) {
            return Array.Exists(
                array,
                element => element.Equals(value)
            );
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
