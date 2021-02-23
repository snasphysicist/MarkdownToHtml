
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class List : IMarkdownParser
    {
        private static Regex regexOrderedListLine = new Regex(
            @"^\s*\d+\.(\s+?.*)"
        );

        private static Regex regexUnorderedListLine = new Regex(
            @"^\s*[\*|\+|-](\s+?.*)"
        );

        private int indentationLevel;

        public List(
            int indentationLevel
        ) {
            this.indentationLevel = indentationLevel;
        }

        public bool CanParseFrom(
            ParseInput input
        ) {
            return IsListItemLine(
                input[0].Text
            ) && (input[0].IndentationLevel() == indentationLevel);
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            ElementType listType = DetermineListType(
                input
            );
            LinkedList<IHtmlable> listItems = new LinkedList<IHtmlable>();
            // Track whether list item contents should be in a paragraph
            bool whitespaceLineBefore = false;
            bool whitespaceLineAfter = false;
            ListItemInner listItemParser = new ListItemInner(
                indentationLevel
            );
            while (
                listItemParser.CanParseFrom(
                    input
                )
            ) {  
                listItemParser.ParseFrom(
                    input
                );
                while (
                    (input.Count > 0)
                    && input[0].HasBeenParsed()
                ) {
                    input.NextLine();
                }
                whitespaceLineAfter = input[-1].Original.ContainsOnlyWhitespace() 
                    && listItemParser.CanParseFrom(
                        input
                    );
                ParseResult listItemResult;
                if (
                    !whitespaceLineBefore 
                    && !whitespaceLineAfter
                    && !listItemParser.ContainsInnerWhitespace
                ) {
                    listItemResult = listItemParser.ContentWithoutParagraphs();
                } else {
                    listItemResult = listItemParser.ContentWithParagraphs();
                }
                foreach (IHtmlable item in listItemResult.GetContent())
                {
                    listItems.AddLast(
                        item
                    );
                }
                // Whitespace after previous entry becomes before next entry
                whitespaceLineBefore = whitespaceLineAfter;
                listItemParser = new ListItemInner(
                    indentationLevel
                );
            }
            Element list = new ElementFactory().New(
                listType,
                listItems.ToArray()
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
    }
}