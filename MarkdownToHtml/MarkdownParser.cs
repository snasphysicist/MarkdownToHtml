
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class MarkdownParser
    {
        
        public bool Success
        { get; private set; }

        public IHtmlable[] Content
        { get; private set; }

        public MarkdownParser(
            string[] lines
        ) {
            // Store parsed content as we go
            LinkedList<IHtmlable> content = new LinkedList<IHtmlable>();
            for (int i = 0; i < lines.Length; i++) 
            {
                // Plain text case
                MarkdownParagraph paragraph = new MarkdownParagraph(
                    ParseSingleLine(lines[i])
                );
                content.AddLast(paragraph);
            }
            Success = true;
            Content = new IHtmlable[content.Count];
            content.CopyTo(Content, 0);
        }

        IHtmlable[] ParseSingleLine(
            string line
        ) {
            // Remove spaces from the start of the line
            while (
                (line.Length > 0)
                && (line.Substring(0, 1) == " ")
            ) {
                line = line.Substring(1);
            }
            // Plain text case, not yet dealing with strong/emph/etc
            return new IHtmlable[]{
                new MarkdownText(line)
            };
        }

    }
}
