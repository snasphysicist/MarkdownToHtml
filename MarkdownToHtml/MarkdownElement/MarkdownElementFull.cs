
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public abstract class MarkdownElementFull : MarkdownElementWithAttributes, IHtmlable
    {

        protected IHtmlable[] content;

        public override string ToHtml() {
            string attributeSection = "";
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                attributeSection += $" {entry.Key}=\"{entry.Value}\"";
            }
            string html = $"<{Tag}{attributeSection}>";
            foreach (IHtmlable htmlable in content)
            {
                html += htmlable.ToHtml();
            }
            html += $"</{Tag}>";
            return html;
        }

    }
}