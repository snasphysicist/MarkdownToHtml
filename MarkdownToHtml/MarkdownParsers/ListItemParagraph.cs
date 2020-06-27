
// namespace MarkdownToHtml
// {
//     public class ListItemParagraph : IMarkdownParser
//     {
//         private IMarkdownParser innerParser;

//         private int indentationLevel;

//         public ListItemParagraph(
//             int indentationLevel
//         ) {
//             this.indentationLevel = indentationLevel;
//             innerParser = new ListItemInner(
//                 indentationLevel
//             );
//         }

//         public bool CanParseFrom(
//             ParseInput input
//         ) {
//             return innerParser.CanParseFrom(
//                 input
//             );
//         }

//         public ParseResult ParseFrom(
//             ParseInput input
//         ) {
//             ParseResult result = new ParseResult();
//             if (!CanParseFrom(input))
//             {
//                 return result;
//             }
//             ParseResult innerContent = innerParser.ParseFrom(
//                 input
//             );
//             if (!innerContent.Success)
//             {
//                 return result;
//             }
//             Element paragraph = new ElementFactory().New(
//                 ElementType.Paragraph,
//                 innerContent.GetContent()
//             );
//             Element listItem = new ElementFactory().New(
//                 ElementType.ListItem,
//                 paragraph
//             );
//             result.Success = true;
//             result.AddContent(
//                 listItem
//             );
//             return result;
//         }
//     }
// }
