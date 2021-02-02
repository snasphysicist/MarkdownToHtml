
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class HtmlElementInserter
    {
        private Dictionary<Guid, string> replacements;

        private string input;

        public string Processed
        { get; private set; }

        public HtmlElementInserter(
            Dictionary<Guid, string> replacements,
            string input
        ) {
            this.replacements = replacements;
            this.input = input;
            Process();
        }

        private void Process()
        {
            Processed = input;
            foreach (Guid placeholder in replacements.Keys)
            {
                input.Replace(
                    placeholder.ToString(),
                    replacements[placeholder]
                );
            }
        }
    }
}