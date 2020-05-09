
namespace MarkdownToHtml
{
    class MarkdownParser
    {
        
        bool Success
        { get; private set; }

        IHtmlable[] Content
        { get; private set; }

        public MarkdownParser(
            string[] lines
        ) {
            Success = false;
            Content = new IHtmlable[]{};
        }

    }
}
