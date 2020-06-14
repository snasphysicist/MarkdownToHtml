
namespace MarkdownToHtml
{
    public class ListItemParagraph : IMarkdownParser
    {
        private static IMarkdownParser listItemParser = new ListItem();

        public bool CanParseFrom(
            ParseInput input
        ) {
            return listItemParser.CanParseFrom(
                input
            );
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            if (
                !CanParseFrom(
                    input
                )
            ) {
                return result;
            }
            ParseResult listItemResult = listItemParser.ParseFrom(
                input
            );
            if (listItemResult.Success) {
                Element listItem = new ElementFactory().New(
                    ElementType.Paragraph,
                    listItemResult.GetContent()
                );
                result.Success = true;
                result.AddContent(
                    listItem
                );
            }
            return result;
        }
    }
}