
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

        // public bool Parse(
        //     out MarkdownElement[] markdownElements
        // ) {
        //     // Need to scan source line by line
        //     string[] lines = Regex.Split(source, "\r\n|\r|\n");
        //     for (int i = 0; i < lines.Length; i++) 
        //     {
        //         // Heading
        //         if (lines[i].StartsWith("#")) 
        //         {

                    
        //         }
        //     }
        // }

    }
}