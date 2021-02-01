
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class HtmlSpecialCharacterEscaper
    {
        private Dictionary<string, string> htmlSpecials = new Dictionary<string, string>
        {
            {"&", "&amp;"},
            {"\"", "&quot;"},
            {"'", "&apos;"},
            {"<", "&lt;"},
            {">", "&gt;"}
        };

        private Regex escapedSpecial = new Regex(
            "^&[a-z]+;"
        );

        private string input;

        public string Escaped;

        public HtmlSpecialCharacterEscaper(
            string input
        ) {
            this.input = input;
            Escape();
        }

        private void Escape()
        {
            Escaped = "";
            int current = 0;
            while (current < input.Length)
            {
                int nextSpecialIndex = FindNextUnescapedSpecialAfter(current);
                Escaped = Escaped + input.Substring(current, nextSpecialIndex);
                current = nextSpecialIndex + 1;
            }
        }

        private int FindNextUnescapedSpecialAfter(
            int current
        ) {
            return input.Length;
        }
    }
}