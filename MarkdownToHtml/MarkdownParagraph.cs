
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class MarkdownParagraph : IHtmlable
    {

        IHtmlable[] content;

        const string tag = "p";

        public const MarkdownElementType Type = MarkdownElementType.Paragraph;

        public MarkdownParagraph(
            IHtmlable[] innerContent
        ) {
            content = innerContent;
        }

        public string ToHtml() 
        {
            string html = $"<{tag}>";
            foreach (IHtmlable htmlable in content)
            {
                html += htmlable.ToHtml();
            }
            html += $"</{tag}>";
            return html;
        }

        // Parse a plain paragraph
        public static ParseResult ParseFrom(
            ParseInput input
        ) {
            ArraySegment<string> lines = input.Lines();
            ParseResult result = new ParseResult();
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            // The paragraph doesn't get parsed past the first blank line
            int endIndex = 0;
            while (
                (endIndex < lines.Count)
                && (
                    !ContainsOnlyWhitespace(
                        lines[endIndex]
                    )
                )
            ) {
                endIndex++;
            }
            int i = 0;
            while (i < endIndex)
            {
                input = input.NextLine();
                lines = input.Lines();
                if (MarkdownCodeBlock.CanParseFrom(input))
                {
                    ParseResult innerResult = MarkdownCodeBlock.ParseFrom(input);
                    foreach (IHtmlable entry in innerResult.GetContent())
                    {
                        innerContent.AddLast(entry);
                    }
                } else {
                    string line = lines[0];
                    if (endsWithAtLeastTwoSpaces(line))
                    {
                        string shortened = StripTrailingWhitespace(line);
                        foreach (
                            IHtmlable entry 
                            in MarkdownParser.ParseInnerText(
                                new ParseInput(
                                    input,
                                    shortened
                                )
                            )
                        ) {
                            innerContent.AddLast(entry);
                        }
                        innerContent.AddLast(
                            new MarkdownLinebreak()
                        );
                    } else {
                        foreach (
                            IHtmlable entry 
                            in MarkdownParser.ParseInnerText(
                                new ParseInput(
                                    input,
                                    line
                                )
                            )
                        ) {
                            innerContent.AddLast(entry);
                        }
                        /*
                        * If this is not the last line,
                        * it doesn't end in a manual linebreak
                        * and the user hasn't added a space themselves
                        * we need to add a space at the end
                        */
                        if (
                            (i < (endIndex - 1))
                            && (line.Length > 0)
                            && (line[^1] != ' ')
                        ) {
                            innerContent.AddLast(
                                new MarkdownText(" ")
                            );
                        }
                    }
                    // Clear the line just consumed
                    lines[0] = "";
                }
                // Move on to next non-empty line
                int j = 0;
                while (
                    (j < lines.Count)
                    && (ContainsOnlyWhitespace(lines[j]))
                ) {
                    j++;
                }
                i += j;
            }
            MarkdownParagraph paragraph = new MarkdownParagraph(
                Utils.LinkedListToArray(innerContent)
            );
            result.Success = true;
            result.AddContent(
                paragraph
            );
            return result;
        }

        private static bool endsWithAtLeastTwoSpaces (
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

        private static string StripTrailingWhitespace(
            string line
        ) {
            return StripTrailingCharacter(
                line,
                ' '
            );
        }

        private static string StripTrailingCharacter(
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

        // Check whether a line contains only whitespace (or is empty)
        private static bool ContainsOnlyWhitespace(
            string line
        ) {
            return line.Replace(
                " ",
                ""
            ).Length == 0;
        }

    }
}
