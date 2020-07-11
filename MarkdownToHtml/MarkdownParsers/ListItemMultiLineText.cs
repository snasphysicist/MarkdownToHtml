
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class ListItemMultiLineText : IMarkdownParser
    {
        private static Regex regexOrderedListLine = new Regex(
            @"^(\s*)(\d+\.)(\s+?.*)"
        );

        private static Regex regexUnorderedListLine = new Regex(
            @"^(\s*)([\*|\+|-])(\s+?.*)"
        );

        private int indentationLevel;

        private static Element linebreak = new ElementFactory().New(
            ElementType.Linebreak
        );

        public ListItemMultiLineText(
            int indentationLevel
        ) {
            this.indentationLevel = indentationLevel;
        }

        public bool CanParseFrom(
            ParseInput input
        ) {
            return indentationLevel == input[0].IndentationLevel();
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            while (input.Count > 0) {
                input[0].Text = input[0].Text.StripLeadingCharacters(
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
                if (
                    input.Count > 0
                    && (
                        input[0].ContainsOnlyWhitespace()
                        || IsListItemLine(
                            input[0].Text
                        )
                    )
                ) {
                    break;
                }
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
            ) && !input[0].ContainsOnlyWhitespace()
            && !IsListItemLine(
                input[1].Text
            );
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

        public bool IsListItemLine(
            string line
        ) {
            return regexOrderedListLine.Match(line).Success
                || regexUnorderedListLine.Match(line).Success;            
        }
    }
}