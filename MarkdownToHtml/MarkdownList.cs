
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownList : IHtmlable
    {

        private static Regex regexOrderedListLine = new Regex(
            @"[\s]{0,3}\d+\.(\s+?.*)"
        );

        private IHtmlable[] content;

        private string tag = "";

        public MarkdownElementType Type
        { get; private set; }

        public MarkdownList(
            IHtmlable[] content,
            MarkdownElementType type
        ) {
            this.content = content;
            Type = type;
            if (Type == MarkdownElementType.OrderedList)
            {
                tag = "ol";
            } else if (Type == MarkdownElementType.UnorderedList)
            {
                tag = "ul";
            }
        }

        public string ToHtml() 
        {
            string html = $"<{tag}>";
            foreach (IHtmlable htmlable in content)
            {
                html += htmlable.ToHtml();
            }
            html += $"</{tag}>";
            return html;
        }

        public static bool CanParseFrom(
            ParseInput input
        ) {
            return regexOrderedListLine.Match(input.FirstLine).Success;
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
            int endQuoteSection = FindEndOfListSection(
                lines
            );
            string[] truncatedLines = new string[endQuoteSection];
            // Remove numbers and spaces, if needed
            for (int i = 0; i < endQuoteSection; i++)
            {
                string truncated = lines[i];
                Match lineContentMatch = regexOrderedListLine.Match(lines[i]);
                if (lineContentMatch.Success)
                {
                    truncated = lineContentMatch.Groups[1].Value;
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
                        truncatedLines[i] = truncated.Substring(spaces);
                    } else {
                        // More than five, just remove one space
                        truncatedLines[i] = truncated.Substring(1);
                    }
                } else {
                    truncatedLines[i] = lines[i];
                }
                // Delete original line, don't parse again
                lines[i] = "";
            }
            // Need to split into groups per list item
            int currentIndex = 0;
            // Hold the parsed list items as we go
            LinkedList<IHtmlable> listItems = new LinkedList<IHtmlable>();
            while (currentIndex < truncatedLines.Length)
            {
                int endIndex = FindEndOfListItem(
                    truncatedLines,
                    currentIndex
                );
                // Should list item contents be in a paragraph?
                bool wrapInParagraph = false;
                // Is preceding line whitespace?
                if (
                    (currentIndex != 0)
                    && (
                        ContainsOnlyWhitespace(
                            truncatedLines[currentIndex - 1]
                        )
                    )
                ) {
                    wrapInParagraph = true;
                }
                // Is following line whitespace?
                if (
                    (endIndex < (truncatedLines.Length - 1))
                    && (
                        ContainsOnlyWhitespace(
                            truncatedLines[endIndex + 1]
                        )
                    )
                ) {
                    wrapInParagraph = true;
                }
                ParseResult nextListItem = MarkdownListItem.ParseFrom(
                    new ParseInput(
                        input.Urls,
                        truncatedLines,
                        currentIndex,
                        endIndex - currentIndex
                    ),
                    wrapInParagraph
                );
                foreach(
                    IHtmlable entry
                    in nextListItem.GetContent()
                ) {
                    listItems.AddLast(
                        entry
                    );
                }
                while (
                    (currentIndex < truncatedLines.Length)
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
                    MarkdownElementType.OrderedList
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
                    && !regexOrderedListLine.Match(lines[index]).Success
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

        private static int FindEndOfListItem(
            string[] lines,
            int startIndex
        ) {
            // The first line (0) will contain a 1. or similar, so skip it
            int endIndex = startIndex + 1;
            while (
                (endIndex < lines.Length)
                && (!regexOrderedListLine.Match(lines[endIndex]).Success)
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
        class MarkdownListItem : IHtmlable {

            private IHtmlable[] content;

            private const string tag = "li";

            public MarkdownElementType Type = MarkdownElementType.ListItem;

            private MarkdownListItem(
                IHtmlable[] content
            ) {
                this.content = content;
            }

            public string ToHtml()
            {
                string html = $"<{tag}>";
                foreach (IHtmlable entry in content)
                {
                    html += entry.ToHtml();
                }
                html += $"</{tag}>";
                return html;
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
                            new IHtmlable[] {
                                new MarkdownParagraph(
                                    innerResult.GetContent()
                                )
                            }
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
