
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class List : IMarkdownParser
    {
        private static IMarkdownParser listItemParser = new ListItem();

        private static Regex regexUnorderedListLine = new Regex(
            @"^[\s]{0,3}[\*|\+|-].*"
        );
        public bool CanParseFrom(
            ParseInput input
        ) {
            return (
                listItemParser.CanParseFrom(
                    input
                )
            );
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            ElementType listType = DeduceListType(
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

        private ElementType DeduceListType(
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
    }
}