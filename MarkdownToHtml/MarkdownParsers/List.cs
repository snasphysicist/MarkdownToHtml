
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class List : IMarkdownParser
    {
        private static IMarkdownParser listItemRawParser = new ListItemRaw();
        private static IMarkdownParser listItemParagraphParser = new ListItemParagraph();

        private static Regex regexOrderedListLine = new Regex(
            @"^[\s]{0,3}\d+\.(\s+?.*)"
        );

        private static Regex regexUnorderedListLine = new Regex(
            @"^[\s]{0,3}[\*|\+|-](\s+?.*)"
        );
        public bool CanParseFrom(
            ParseInput input
        ) {
            return IsListItemLine(
                input[0].Text
            );
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            ElementType listType = DetermineListType(
                input
            );
            IMarkdownParser listItemParser = DetermineListItemParserType(
                input
            );
            ParseResult parsedListItem;
            LinkedList<IHtmlable> listItems = new LinkedList<IHtmlable>();
            while (
                listItemParser.CanParseFrom(
                    input
                )
            ) {
                parsedListItem = listItemParser.ParseFrom(
                    input
                );
                foreach (IHtmlable item in parsedListItem.GetContent())
                {
                    listItems.AddLast(
                        item
                    );
                }
                while (
                    (input.Count > 0)
                    && input[0].HasBeenParsed()
                ) {
                    input.NextLine();
                }
            }
            Element list = new ElementFactory().New(
                listType,
                Utils.LinkedListToArray(
                    listItems
                )
            );
            result.Success = true;
            result.AddContent(
                list
            );
            return result;
        }

        private bool IsListItemLine(
            string line
        ) {
            return (
                regexOrderedListLine.Match(line).Success
                || regexUnorderedListLine.Match(line).Success
            );
        }

        private ElementType DetermineListType(
            ParseInput input
        ) {
            if (
                regexUnorderedListLine.Match(
                    input[0].Text
                ).Success
            ) {
                return ElementType.UnorderedList;
            } else {
                return ElementType.OrderedList;
            }
        }

        private IMarkdownParser DetermineListItemParserType(
            ParseInput input
        ) {
            if (
                ListItemsWrappedInParagraphs(
                    input
                )
            ) {
                return listItemParagraphParser;
            } else {
                return listItemRawParser;
            }
        }

        private bool ListItemsWrappedInParagraphs(
            ParseInput input
        ) {
            // Note: includes trailing whitespace lines
            int endOfListSection = FindEndOfListSection(
                input
            );
            // Step back one line if at end of input
            if (endOfListSection == input.Count)
            {
                endOfListSection--;
            }
            // Step back to ignore trailing whitespace lines
            while (
                input[endOfListSection].ContainsOnlyWhitespace()
            ) {
                endOfListSection--;
            }
            return ContainsWhitespaceLine(
                input,
                endOfListSection
            );
        }

        private bool ContainsWhitespaceLine(
            ParseInput input,
            int checkUpTo
        ) {
            for (int i = 0; i < checkUpTo; i++)
            {
                if (input[i].ContainsOnlyWhitespace())
                {
                    return true;
                }
            }
            return false;
        }

        private int FindEndOfListSection(
            ParseInput input
        ) {
            int currentIndex = 0;
            while (
                (currentIndex < input.Count)
                && ExistsSubsequentItemInList(
                    input,
                    currentIndex
                )
            ) {
                currentIndex = FindStartOfNextListItem(
                    input,
                    currentIndex
                );
                currentIndex = FindEndOfListItem(
                    input,
                    currentIndex
                );
            }
            return currentIndex;
        }

        private bool ExistsSubsequentItemInList(
            ParseInput input,
            int checkFrom
        ) {
            int index = checkFrom;
            while (
                index < input.Count
            ) {
                if (
                    IsListItemLine(
                        input[index].Text
                    )
                ) {
                    break;
                }
                if (!input[index].ContainsOnlyWhitespace())
                {
                    return false;
                }
                index++;
            }
            return index < input.Count;
        }

        private int FindStartOfNextListItem(
            ParseInput input,
            int startSearchFrom
        ) {
            int index = startSearchFrom;
            while (
                index < input.Count
            ) {
                if (
                    IsListItemLine(
                        input[index].Text
                    )
                ) {
                    break;
                }
                index++;
            }
            return index;
        }

        private int FindEndOfListItem(
            ParseInput input,
            int startIndex
        ) {
            int currentLine = startIndex + 1;
            while (
                (currentLine < input.Count)
                && !input[currentLine].ContainsOnlyWhitespace()
                && !IsListItemLine(
                    input[currentLine].Text
                ) 
            ) {
                currentLine++;
            }
            return currentLine;
        }
    }
}