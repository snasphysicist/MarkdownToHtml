
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
            return regexOrderedListLine.Match(input[0].Text).Success
                || regexUnorderedListLine.Match(input[0].Text).Success;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            Match orderedListItemContent = regexOrderedListLine.Match(input.FirstLine);
            string innerText = ;
            if (orderedListItemContent.Success)
            {

            } else {

            }
            bool wrapInParagraph = WhitespaceLinePreceedsOrFollowsThisItem(
                input
            );
            Element listItem;
            if (
                !input.AtLastLine()
                && CanParseFrom(
                    input.NextLine()
                )
            ) {
                listItem = new ElementFactory().New(
                    ElementType.ListItem,
                    MarkdownParser.ParseInnerText(

                    )
                )
            }

            if (wrapInParagraph)
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
            result.Success = true;
            result.AddContent(
                returnedElement
            );
            return result;
        }
    }
}