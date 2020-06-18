
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class ListItemInner : IMarkdownParser
    {
        private static Regex regexOrderedListLine = new Regex(
            @"^\s*\d+\.(\s+?.*)"
        );

        private static Regex regexUnorderedListLine = new Regex(
            @"^\s*[\*|\+|-](\s+?.*)"
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
            int listItemIdentationLevel = CalculateIndentationLevel(
                input[0].Text
            );
            int endOfListItem = FindEndOfListItem(
                input,
                listItemIdentationLevel
            );
            int startOfNestedList = FindStartOfNestedList(
                input,
                listItemIdentationLevel
            );
            input[0].Text = RemoveListIndicator(
                input[0].Text
            );
            if (startOfNestedList < endOfListItem) 
            {
                ParseResult innerText = new MultiLineText().ParseFrom(
                    input.LinesFromStart(
                        startOfNestedList
                    )
                );
                ParseResult nestedList = new List().ParseFrom(
                    RemoveIndentLevel(
                        input.JumpLines(
                            startOfNestedList
                        ).LinesFromStart(
                            endOfListItem - startOfNestedList
                        )
                    )
                );
                foreach (IHtmlable item in innerText.GetContent())
                {
                    result.AddContent(
                        item
                    );
                }
                foreach (IHtmlable item in nestedList.GetContent())
                {
                    result.AddContent(
                        item
                    );
                }
            } else {
                ParseResult innerText = new MultiLineText().ParseFrom(
                    input.LinesFromStart(
                        endOfListItem
                    )
                );
                foreach (IHtmlable item in innerText.GetContent())
                {
                    result.AddContent(
                        item
                    );
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

        private int FindEndOfListItem(
            ParseInput input,
            int outerListIndentationLevel
        ) {
            int i = 1;
            while (
                (i < input.Count)
                && (
                    !IsListItemLineAtIndentationLevel(
                        input[i].Text,
                        outerListIndentationLevel
                    )
                )
            ) {
                i++;
            }
            return i;
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

        private int FindStartOfNestedList(
            ParseInput input,
            int containingListItemIdentationLevel
        ) {
            int lineNumber = 0;
            while(
                lineNumber < input.Count
                && !IsListItemLineAtIndentationLevel(
                    input[lineNumber].Text,
                    containingListItemIdentationLevel + 1
                )
            ) {
                lineNumber++;
            }
            return lineNumber;
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
