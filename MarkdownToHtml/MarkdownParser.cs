
namespace MarkdownToHtml
{
    class MarkdownParser
    {
        
        public bool Success
        { get; private set; }

        public IHtmlable[] Content
        { get; private set; }

        public MarkdownParser(
            string[] lines
        ) {
            Success = false;
            Content = new IHtmlable[]{};
        }

    }
}
