
// namespace MarkdownToHtml
// {
//     public class ListItemRaw : IMarkdownParser
//     {
//         private IMarkdownParser innerParser;

//         private int indentationLevel;

//         public ListItemRaw(
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
//             Element listItem = new ElementFactory().New(
//                 ElementType.ListItem,
//                 innerContent.GetContent()
//             );
//             result.Success = true;
//             result.AddContent(
//                 listItem
//             );
//             return result;
//         }
//     }
// }