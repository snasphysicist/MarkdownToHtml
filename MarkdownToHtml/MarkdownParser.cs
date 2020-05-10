
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownParser
    {

        // Regex matches
        Regex regexHorizontalLine = new Regex(@"\*\*\*|---|___");

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
                currentIndex += lineGroup.Count;
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
            int endIndex = startIndex;
            while (
                (endIndex < lines.Length)
                && (!containsOnlyWhitespace(
                    lines[endIndex]
                ))
            ) {
                endIndex++;
            }
            int elements = endIndex - startIndex;
            return new ArraySegment<string>(
                lines, 
                startIndex, 
                elements
            );
        }

        // Check whether a line contains only whitespace (or is empty)
        private bool containsOnlyWhitespace(
            string line
        ) {
            return line.Replace(
                " ",
                ""
            ).Length == 0;
        }

        private bool ParseLineGroup(
            ArraySegment<string> lines,
            LinkedList<IHtmlable> content
        ) {
            string firstLine = lines[0];
            if (firstLine.StartsWith("#")) {
                // Single line heading
                return ParseSingleLineHeading(
                    lines[0],
                    content
                );
            } else if (
                (firstLine.Length > 2)
                && regexHorizontalLine.IsMatch(
                    firstLine.Substring(0, 3)
                )
            ) {
                return ParseHorizontalRule(
                    lines[0],
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
                    isInArray(
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
                    string shortened = stripTrailingWhitespace(line);
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
                        && (line[^1] != ' ')
                    ) {
                        innerContent.AddLast(
                            new MarkdownText(" ")
                        );
                    }
                }
            }
            MarkdownParagraph paragraph = new MarkdownParagraph(innerContent);
            content.AddLast(
                paragraph
            );
            return true;
        }

        private bool endsWithAtLeastTwoSpaces (
            string line
        ) {
            return line.Substring(
                line.Length - 2,
                2
            ) == "  ";
        }

        private string stripTrailingWhitespace(
            string line
        ) {
            while (
                (line.Length > 0)
                && (line[^1] == ' ')
            ) {
                line = line.Substring(
                    0,
                    line.Length - 1 
                );
            }
            return line;
        }

        // Parse a single line heading
        private bool ParseSingleLineHeading(
            string line,
            LinkedList<IHtmlable> content
        ) {
            // Work out heading level
            int level = 0;
            while(
                level < (line.Length)
                && (line[level] == '#')
            ) {
                level++;
            }
            /* 
             * If heading level allowed (1-6)
             * and there is at least one more character except hashes
             * and hashes followed by space
             * then line can be parsed as heading
             */
            if (
                (level > 0) 
                && (level < 7)
                && (line.Length > level)
                && (line[level] == ' ')
            ) {
                // Parse as heading
                content.AddLast(
                    new MarkdownHeading(
                        level,
                        ParseInnerText(
                            line.Substring(level + 1)
                        )
                    )
                );
                return true;
            } else {
                // Invalid heading syntax, assume it's just a paragraph
                return ParseParagraph(
                    new ArraySegment<string>(
                        new string[]
                        {
                            line
                        }, 
                        0, 
                        1
                    ),
                    content
                );
            }
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

        // Check whether a value is in an array
        private bool isInArray<T>(
            T value,
            T[] array
        ) {
            return Array.Exists(
                array,
                element => element.Equals(value)
            );
        }
    }
}
