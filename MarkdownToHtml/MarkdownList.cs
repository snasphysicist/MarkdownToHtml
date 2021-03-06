
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownList : MarkdownElementWithContent, IHtmlable
    {

        private static Regex regexOrderedListLine = new Regex(
            @"^[\s]{0,3}\d+\.(\s+?.*)"
        );

        private static Regex regexUnorderedListLine = new Regex(
            @"^[\s]{0,3}[\*|\+|-](\s+?.*)"
        );

        public MarkdownList(
            IHtmlable[] content,
            MarkdownElementType type
        ) {
            this.content = content;
            Type = type;
        }

        public static bool CanParseFrom(
            ParseInput input
        ) {
            return (
                regexOrderedListLine.Match(input.FirstLine).Success
                || regexUnorderedListLine.Match(input.FirstLine).Success
            );
        }

        public static ParseResult ParseFrom(
            ParseInput input
        ) {
            ArraySegment<string> lines = input.Lines();
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            /* 
             * Work out the list type from the first line
             * before we start to mangle the line contents
             */
            MarkdownElementType type;
            if (regexOrderedListLine.Match(input.FirstLine).Success)
            {
                type = MarkdownElementType.OrderedList;
            } else
            {
                type = MarkdownElementType.UnorderedList;
            }
            int endListSection = FindEndOfListSection(
                lines
            );
            // New array segment with only parsed content
            ArraySegment<string> listLines = new ArraySegment<string>(
                lines.Array,
                lines.Offset,
                endListSection
            );
            // Need to split into groups per list item
            int currentIndex = 0;
            // Hold the parsed list items as we go
            LinkedList<IHtmlable> listItems = new LinkedList<IHtmlable>();
            // Track whether list item contents should be in a paragraph
            bool whitespaceLineBefore = false;
            bool whitespaceLineAfter = false;
            while (currentIndex < endListSection)
            {
                int endIndex = FindEndOfListItem(
                    listLines,
                    currentIndex
                );
                /*
                 * There is a whitespace line between
                 * this list item and the following one
                 * and this list item isn't the final one
                 */
                whitespaceLineAfter = (
                    (endIndex < listLines.Count)
                    && (
                        ContainsOnlyWhitespace(
                            listLines[endIndex - 1]
                        )
                    )
                );
                // Create new parse input for this list item
                ParseInput listItemLines = new ParseInput(
                    input.Urls,
                    listLines.Array,
                    listLines.Offset + currentIndex,
                    endIndex - currentIndex
                );
                RemoveListIndicators(
                    listItemLines
                );
                ParseResult nextListItem = MarkdownListItem.ParseFrom(
                    listItemLines,
                    whitespaceLineBefore || whitespaceLineAfter
                );
                foreach(
                    IHtmlable entry
                    in nextListItem.GetContent()
                ) {
                    listItems.AddLast(
                        entry
                    );
                }
                // Jump over lines just parsed
                currentIndex += (endIndex - currentIndex);
                // Whitespace after previous entry becomes before next entry
                whitespaceLineBefore = whitespaceLineAfter;
                /* 
                 * If there's further whitespace after parsed section
                 * (for some reason) then jump over this too
                 */
                while (
                    (currentIndex < listLines.Count)
                    && (
                        ContainsOnlyWhitespace(
                        lines[currentIndex]
                        )
                    )
                ) {
                    currentIndex++;
                }
            }
            result.Success = true;
            result.AddContent(
                new MarkdownList(
                    Utils.LinkedListToArray(
                        listItems
                    ),
                    type
                )
            );
            return result;
        }

        private static int FindEndOfListSection(
            ArraySegment<string> lines
        ) {
            int index = 1;
            bool previousLineWasWhitespace = false;
            /* 
             * Condition
             * Don't allow index to exceed the number of elements to avoid Exceptions
             * We want to break the loop when there is a whitespace line
             * (previousLineWasWhitespace)
             * followed by a non-whitespace line (!ContainsOnlyWhitespace(lines[index])) 
             * which is not a list line !regex....Match(line).Success
             */
            while (
                index < lines.Count
                && !(
                    previousLineWasWhitespace
                    && !ContainsOnlyWhitespace(lines[index])
                    && !(
                        regexOrderedListLine.Match(lines[index]).Success
                        || regexUnorderedListLine.Match(lines[index]).Success
                    )
                )
            ) {
                if (ContainsOnlyWhitespace(lines[index]))
                {
                    previousLineWasWhitespace = true;
                } else {
                    previousLineWasWhitespace = false;
                }
                index++;
            }
            return index;
        }

        private static void RemoveListIndicators(
            ParseInput input
        ) {
            ArraySegment<string> lines = input.Lines();
            for (int i = 0; i < lines.Count; i++)
            {
                string truncated = lines[i];
                Match lineOrderedContentMatch = regexOrderedListLine.Match(lines[i]);
                Match lineUnorderedContentMatch = regexUnorderedListLine.Match(lines[i]);
                if (
                    lineOrderedContentMatch.Success
                    || lineUnorderedContentMatch.Success
                )
                {
                    if (lineOrderedContentMatch.Success) {
                        truncated = lineOrderedContentMatch.Groups[1].Value;
                    } else if (lineUnorderedContentMatch.Success)
                    {
                        truncated = lineUnorderedContentMatch.Groups[1].Value;
                    }
                    int spaces = 0;
                    // Count spaces
                    while(
                        (spaces < truncated.Length)
                        && (truncated[spaces] == ' ')
                    ) {
                        spaces++;
                    }
                    // If there are fewer than 5 spaces, remove all
                    if (spaces < 5)
                    {
                        lines[i] = truncated.Substring(spaces);
                    } else {
                        // More than five, just remove one space
                        lines[i] = truncated.Substring(1);
                    }
                } else {
                    lines[i] = lines[i];
                }
            }
        }

        private static int FindEndOfListItem(
            ArraySegment<string> lines,
            int startIndex
        ) {
            // The first line (0) will contain a 1. or similar, so skip it
            int endIndex = startIndex + 1;
            while (
                (endIndex < lines.Count)
                && !(
                    regexOrderedListLine.Match(lines[endIndex]).Success
                    || regexUnorderedListLine.Match(lines[endIndex]).Success
                )
            ) {
                endIndex++;
            }
            return endIndex;
        }

        private static bool ContainsOnlyWhitespace(
            string line
        ) {
            return line.Replace(
                " ",
                ""
            ).Length == 0;
        }

        // List item inner class = o
        class MarkdownListItem : MarkdownElementWithContent, IHtmlable 
        {

            private MarkdownListItem(
                IHtmlable[] content
            ) {
                Type = MarkdownElementType.ListItem;
                this.content = content;
            }

            public static ParseResult ParseFrom(
                ParseInput lines,
                bool innerParagraph
            ) {
                ParseResult result = new ParseResult();
                ParseResult innerResult;
                IHtmlable returnedElement;
                // If the list items content contains another list
                if (MarkdownList.CanParseFrom(lines))
                {
                    innerResult = MarkdownList.ParseFrom(lines);
                    returnedElement = new MarkdownListItem(
                        innerResult.GetContent()
                    );
                } else 
                {
                    // Otherwise, if the item content should go in a paragraph
                    if (innerParagraph)
                    {
                        innerResult = MarkdownParagraph.ParseFrom(lines);
                        returnedElement = new MarkdownListItem(
                            innerResult.GetContent()
                        );
                    } else 
                    {
                        // line item content should not go in a paragraph
                        returnedElement = new MarkdownListItem(
                            MarkdownParser.ParseInnerText(lines)
                        );
                    }
                }
                result.Success = true;
                result.AddContent(
                    returnedElement
                );
                return result;
            }
            
        } // End of inner class

    }
}
