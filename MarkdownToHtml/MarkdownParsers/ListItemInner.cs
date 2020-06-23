
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class ListItemInner : IMarkdownParser
    {
        private static Regex regexOrderedListLine = new Regex(
            @"^(\s*)(\d+\.)(\s+?.*)"
        );

        private static Regex regexUnorderedListLine = new Regex(
            @"^(\s*)([\*|\+|-])(\s+?.*)"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            return (input.Count > 0)
                && IsListItemLine(input[0].Text);
        }

        public bool IsListItemLine(
            string line
        ) {
            return regexOrderedListLine.Match(line).Success
                || regexUnorderedListLine.Match(line).Success;            
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            int indentationLevel = CalculateIndentationLevel(
                input[0].Text
            );
            input[0].Text = RemoveListIndicator(
                input[0].Text
            );
            while(
                input.Count > 0
                && IsPartOfListItem(
                    input,
                    indentationLevel
                )
            ) {
                ParseResult innerResult = new MultiLineText(
                    indentationLevel
                ).ParseFrom(
                    input
                );
                foreach (IHtmlable entry in innerResult.GetContent())
                {
                    result.AddContent(
                        entry
                    );
                }
                while(
                    input.Count > 0
                    && (
                        input[0].ContainsOnlyWhitespace()
                        || input[0].HasBeenParsed()
                    )
                ) {
                    input.NextLine();
                }
            }
            result.Success = true;
            return result;
        }

        private int CalculateIndentationLevel(
            string listItemLine
        ) {
            return (
                listItemLine.Length - Utils.StripLeadingCharacter(
                    listItemLine,
                    ' '
                ).Length
            ) / 4;
        }

        private bool IsPartOfListItem(
            ParseInput input,
            int level
        ) {
            return (
                CalculateIndentationLevel(
                    input[0].Text
                ) >= level
            ) && !IsListItemLine(
                input[0].Text
            );
        }

        private bool IsListItemLineAtIndentationLevel(
            string line,
            int indentationLevel
        ) {
            return IsListItemLine(
                line
            ) && (
                indentationLevel == CalculateIndentationLevel(
                    line
                )
            );
        }

        private bool IsWhitespaceLineAdjacent(
            ParseInput input
        ) {
            bool foundAdjacentWhitespaceLine = false;
            try
            {
                foundAdjacentWhitespaceLine =
                    foundAdjacentWhitespaceLine || input[-1].ContainsOnlyWhitespace();
            } catch (IndexOutOfRangeException)
            {
                // We were at the start of the input, so can't be preceded by whitespace
            }
            try
            {
                foundAdjacentWhitespaceLine =
                    foundAdjacentWhitespaceLine || input[1].ContainsOnlyWhitespace();
            } catch (IndexOutOfRangeException)
            {
                // We were at the end of the input, so can't be followed by whitespace
            }
            return foundAdjacentWhitespaceLine;
        }

        private bool ContainsWhitespaceLine(
            ParseInput input,
            int endIndex
        ) {
            for (int i = 0; i  < endIndex; i++)
            {
                if (input[i].ContainsOnlyWhitespace())
                {
                    return true;
                }
            }
            return false;
        }

        public string RemoveListIndicator(
            string line
        ) {
            if (
                regexOrderedListLine.Match(
                    line
                ).Success
            ) {
                return regexOrderedListLine.Replace(
                    line,
                    "$1$3"
                );
            } else 
            {
                return regexUnorderedListLine.Replace(
                    line,
                    "$1$3"
                );
            }
        }

        private ParseInput RemoveIndentLevel(
            ParseInput input
        ) {
            for (int i = 0; i < input.Count; i++)
            {
                input[i].Text = Utils.StripLeadingCharacterUpTo(
                    input[i].Text,
                    ' ',
                    4
                );
            }
            return input;
        }
    }
}
