
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class MultiLineText : IMarkdownParser
    {
        private static Element linebreak = new ElementFactory().New(
            ElementType.Linebreak
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            return true;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            while (
                !input[0].ContainsOnlyWhitespace()
            ) {
                if (new CodeBlock().CanParseFrom(input))
                {
                    ParseResult innerResult = new CodeBlock().ParseFrom(input);
                    foreach (IHtmlable entry in innerResult.GetContent())
                    {
                        result.AddContent(entry);
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
                            result.AddContent(entry);
                        }
                        result.AddContent(
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
                            result.AddContent(entry);
                        }
                        /*
                        * If this is not the last line,
                        * it doesn't end in a manual linebreak
                        * and the user hasn't added a space themselves
                        * we need to add a space at the end
                        */
                        if (
                            !AtParagraphLastLine(
                                input
                            )
                            && (line.Length > 0)
                            && !line.EndsWith(
                                ' '
                            )
                        ) {
                            result.AddContent(
                                new MarkdownText(" ")
                            );
                        }
                    }
                    // Clear the line just consumed
                    input[0].WasParsed();
                }
                // Move on to next un-parsed line
                while (
                    (input.Count > 0)
                    && (input[0].HasBeenParsed())
                ) {
                    input.NextLine();
                }
            }
            result.Success = true;
            return result;
        }

        private bool AtParagraphLastLine(
            ParseInput input
        ) {
            return (input.Count > 1)
                && (!input[1].ContainsOnlyWhitespace());
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
    }
}