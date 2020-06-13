
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class Paragraph : IMarkdownParser
    {
        private static Element linebreak = new ElementFactory().New(
            ElementType.Linebreak
        );

        public bool CanParseFrom(
            ParseInput input
        )
        {
            return true;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            // The paragraph doesn't get parsed past the first blank line
            int endIndex = 0;
            while (
                (endIndex < input.Count)
                && (
                    !input[endIndex].ContainsOnlyWhitespace()
                )
            ) {
                endIndex++;
            }
            int i = 0;
            while (i < endIndex)
            {
                if (new CodeBlock().CanParseFrom(input))
                {
                    ParseResult innerResult = new CodeBlock().ParseFrom(input);
                    foreach (IHtmlable entry in innerResult.GetContent())
                    {
                        innerContent.AddLast(entry);
                    }
                } else {
                    // Always remove leading spaces
                    string line = Utils.StripLeadingCharacter(
                        input[0].Text,
                        ' '
                    );
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
                            linebreak
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
                    input[0].WasParsed();
                }
                // Move on to next non-empty line
                int j = 0;
                while (
                    (j < input.Count)
                    && (input[j].ContainsOnlyWhitespace())
                ) {
                    j++;
                }
                i += j;
                // Move array slice to next non-empty line
                input = input.JumpLines(j);
            }
            Element paragraph = new ElementFactory().New(
                ElementType.Paragraph,
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