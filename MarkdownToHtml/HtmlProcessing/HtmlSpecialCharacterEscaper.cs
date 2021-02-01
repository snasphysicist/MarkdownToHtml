
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
            "^&[a-z0-9]+;"
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
                Escaped = Escaped + input.Substring(current, nextSpecialIndex - current);
                if (nextSpecialIndex < input.Length)
                {
                    string escapeText = htmlSpecials[input.Substring(nextSpecialIndex, 1)];
                    Escaped = Escaped + escapeText;
                    current = nextSpecialIndex + escapeText.Length; 
                }
                current = nextSpecialIndex + 1;
            }
        }

        private int FindNextUnescapedSpecialAfter(
            int current
        ) {
            while (
                current < input.Length 
                && (
                    !IsHtmlSpecialCharacterAtStart(input.Substring(current))
                    || EscapedSpecialAtStart(input.Substring(current))
                )
            ) {
                current++;
            }
            return current;
        }

        private bool IsHtmlSpecialCharacterAtStart(
            string checkStart
        ) {
            return htmlSpecials.ContainsKey(checkStart.Substring(0, 1));
        }

        private bool EscapedSpecialAtStart(
            string checkStart
        ) {
            return escapedSpecial.IsMatch(checkStart);
        }
    }
}