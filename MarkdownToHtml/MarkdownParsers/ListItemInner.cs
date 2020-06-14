
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class ListItemInner : IMarkdownParser
    {
        private static Regex regexOrderedListLine = new Regex(
            @"^[\s]{0,3}\d+\.(\s+?.*)"
        );

        private static Regex regexUnorderedListLine = new Regex(
            @"^[\s]{0,3}[\*|\+|-](\s+?.*)"
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
            int endOfListItem = FindEndOfListItem(
                input
            );
            // Remove list indicator from first line
            input[0].Text = RemoveListIndicator(
                input[0].Text
            );
            return new MultiLineText().ParseFrom(
                input.LinesFromStart(
                    endOfListItem
                )
            );
        }

        private int FindEndOfListItem(
            ParseInput input
        ) {
            int i = 1;
            while (
                (i < input.Count)
                && (
                    !IsListItemLine(
                        input[i].Text
                    )
                )
            ) {
                i++;
            }
            return i;
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
            Match orderedListItemContent = regexOrderedListLine.Match(
                line
            );
            Match unorderedListItemContent = regexUnorderedListLine.Match(
                line
            );
            return (
                orderedListItemContent.Groups[1].Value
                + unorderedListItemContent.Groups[1].Value
            );
        }
    }
}
