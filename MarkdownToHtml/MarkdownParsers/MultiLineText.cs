
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class MultiLineText : IMarkdownParser
    {
        private int indentationLevel;

        private static Element linebreak = new ElementFactory().New(
            ElementType.Linebreak
        );

        public MultiLineText(
            int indentationLevel
        ) {
            this.indentationLevel = indentationLevel;
        }

        public bool CanParseFrom(
            ParseInput input
        ) {
            int indentationSpaces 
                = input[0].Text.Length - Utils.StripLeadingCharacter(
                    input[0].Text,
                    ' '
                ).Length;
            return indentationLevel == (indentationSpaces / 4);
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            while (
                (input.Count > 0)
                && !input[0].ContainsOnlyWhitespace()
            ) {
                input[0].Text = Utils.StripLeadingCharacter(
                    input[0].Text,
                    ' '
                );
                if (endsWithAtLeastTwoSpaces(input[0].Text))
                {
                    string shortened = StripTrailingWhitespace(input[0].Text);
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
                            input
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
                        AddSpaceToEndOfLine(
                            input
                        )
                    ) {
                        result.AddContent(
                            new MarkdownText(" ")
                        );
                    }
                }
                // Clear the line just consumed
                input[0].WasParsed();
                input.NextLine();
            }
            result.Success = true;
            return result;
        }

        private bool AtParagraphLastLine(
            ParseInput input
        ) {
            return (input.Count < 2)
                || (
                    (input.Count > 1)
                    && (input[1].ContainsOnlyWhitespace())
                );
        }

        private bool AddSpaceToEndOfLine(
            ParseInput input
        ) {
            return !AtParagraphLastLine(
                input
            ) && !input[0].EndsWith(
                " "
            ) && !input[0].ContainsOnlyWhitespace();
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