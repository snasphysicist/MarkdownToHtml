
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class ListItem : IMarkdownParser
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
            return IsListItemLine(input[0].Text);
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
            // Find end of ListItem
            int endOfListItem = FindEndOfListItem(
                input
            );
            // AdjacentToWhitespace?
            bool wrapInParagraph = IsWhitespaceLineAdjacent(
                input
            );
            // ContainsWhitespace?
            wrapInParagraph = wrapInParagraph
                || ContainsWhitespaceLine(
                    input,
                    endOfListItem
            );
            // ParseAsParagraph

            // Match orderedListItemContent = regexOrderedListLine.Match(input.FirstLine);
            // string innerText = ;
            // if (orderedListItemContent.Success)
            // {

            // } else {

            // }
            // bool wrapInParagraph = WhitespaceLinePreceedsOrFollowsThisItem(
            //     input
            // );
            // Element listItem;
            // if (
            //     !input.AtLastLine()
            //     && CanParseFrom(
            //         input.NextLine()
            //     )
            // ) {
            //     listItem = new ElementFactory().New(
            //         ElementType.ListItem,
            //         MarkdownParser.ParseInnerText(

            //         )
            //     )
            // }

            // if (wrapInParagraph)
            // {
            //     innerResult = MarkdownParagraph.ParseFrom(lines);
            //     returnedElement = new MarkdownListItem(
            //         innerResult.GetContent()
            //     );
            // } else 
            // {
            //     // line item content should not go in a paragraph
            //     returnedElement = new MarkdownListItem(
            //         MarkdownParser.ParseInnerText(lines)
            //     );
            // }
            // result.Success = true;
            // result.AddContent(
            //     returnedElement
            // );
            // return result;
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
    }
}