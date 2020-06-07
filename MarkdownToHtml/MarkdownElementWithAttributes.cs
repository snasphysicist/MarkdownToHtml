
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public abstract class MarkdownElementWithAttributes : MarkdownElementBase, IHtmlable
    {

        protected Dictionary<string, string> attributes;

        public override string ToHtml() {
            string attributeSection = "";
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                attributeSection += $" {entry.Key}=\"{entry.Value}\"";
            }
            string html = $"<{Tag}{attributeSection}>";
            html += $"</{Tag}>";
            return html;
        }

    }
}