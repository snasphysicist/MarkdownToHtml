
namespace MarkdownToHtml
{
    public interface IMarkdownParser
    {
        public bool CanParseFrom(
            ParseInput input
        );
        public ParseResult ParseFrom(
            ParseInput input
        );
    }
}