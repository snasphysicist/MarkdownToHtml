
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class MarkdownParser
    {

        public bool Success
        { get; private set; }

        public IHtmlable[] Content
        { get; private set; }

        public MarkdownParser(
            string[] lines
        ) {
            // Assume success
            Success = true;
            // Store parsed content as we go
            LinkedList<IHtmlable> content = new LinkedList<IHtmlable>();
            for (int i = 0; i < lines.Length; i++) 
            {
                bool success;
                // Line is a single line heading
                if (lines[i].StartsWith("#"))
                {
                    success = ParseSingleLineHeading(
                        lines[i],
                        content
                    );
                } else {
                    // Plain text case
                    success = ParseParagraph(
                        lines[i],
                        content
                    );
                }
            }
            Content = new IHtmlable[content.Count];
            content.CopyTo(Content, 0);
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
            string line,
            LinkedList<IHtmlable> content
        ) {
            MarkdownParagraph paragraph = new MarkdownParagraph(
                ParseInnerText(line)
            );
            content.AddLast(
                paragraph
            );
            return true;
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
                    line,
                    content
                );
            }
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
