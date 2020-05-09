
namespace MarkdownToHtml
{
    class MarkdownDocument
    {

        string Source
        { get; private set; }

        IHtmlable[] content;

        bool Success
        { get; private set; }

        public MarkdownDocument(
            string source
        )
        {
            Source = source;
            Success = false;
        }

        

    }
}