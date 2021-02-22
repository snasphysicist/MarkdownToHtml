
namespace MarkdownToHtml
{
    public abstract class HtmlWriterBase
    {
        protected string TerminateWithNewLineIfRequired(
            ElementDetails details, 
            string html
        ) {
            if (details.NewLine == ElementDetails.FollowWithNewLine.Yes) 
            {
                return html + "\n";
            }
            return html;
        }
    }
}