
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// TODO
// Record which subitems are multiline text and which are not
// Offer method to get content with/without paragraphs around multiline text

namespace MarkdownToHtml
{
    public class ListItemInner
    {
        private static Regex regexOrderedListLine = new Regex(
            @"^(\s*)(\d+\.)(\s+?.*)"
        );

        private static Regex regexUnorderedListLine = new Regex(
            @"^(\s*)([\*|\+|-])(\s+?.*)"
        );

        private int indentationLevel;

        private Dictionary<int, IHtmlable[]> paragraphable;

        private Dictionary<int, IHtmlable[]> notParagraphable;

        int numberOfElements;

        public bool ContainsInnerWhitespace
        { get; private set; }

        public ListItemInner(
            int indentationLevel
        ) {
            this.indentationLevel = indentationLevel;
            ContainsInnerWhitespace = false;
            paragraphable = new Dictionary<int, IHtmlable[]>();
            notParagraphable = new Dictionary<int, IHtmlable[]>();
            numberOfElements = 0;
        }

        public bool CanParseFrom(
            ParseInput input
        ) {
            return (input.Count > 0)
                && IsListItemLine(input[0].Text)
                && (
                    CalculateIndentationLevel(
                        input[0].Text
                    ) == indentationLevel
                );
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
            IMarkdownParser listParser = new List(
                indentationLevel + 1
            );
            bool whitespaceLineBefore = false;
            bool whitespaceLineAfter = false;
            do 
            {
                ParseResult innerResult;
                if (
                    listParser.CanParseFrom(
                        input
                    )
                ) {
                    innerResult = listParser.ParseFrom(
                        input
                    );
                    notParagraphable.Add(
                        numberOfElements,
                        innerResult.GetContent()
                    );
                    numberOfElements++;
                } else 
                {
                    innerResult = new ListItemMultiLineText(
                        indentationLevel
                    ).ParseFrom(
                        input
                    );
                    paragraphable.Add(
                        numberOfElements,
                        innerResult.GetContent()
                    );
                    numberOfElements++;
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
                // Whitespace after previous entry becomes before next entry
                whitespaceLineBefore = whitespaceLineAfter;
                ContainsInnerWhitespace = ContainsInnerWhitespace || whitespaceLineBefore;
                if (input[-1].ContainsOnlyWhitespace())
                {
                    whitespaceLineAfter = true;
                }
            } while (
                input.Count > 0
                && IsPartOfListItem(
                    input,
                    indentationLevel + 1
                )
            );
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
            ) || (
                CalculateIndentationLevel(
                    input[0].Text
                ) == level
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
    }
}
