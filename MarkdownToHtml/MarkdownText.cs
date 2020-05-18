
using System;

namespace MarkdownToHtml
{
    public class MarkdownText : IHtmlable
    {

        private static string[][] EscapeReplacements = new string[][]{
            new string[] {"\\*", "*"},
            new string[] {"\\_", "_"},
            new string[] {"\\~", "~"},
            new string[] {"\\`", "`"}
        };

        private static char[] specialCharacters = new char[] {
            '*',
            '_',
            '~',
            '`'
        };

        string content;

        public MarkdownElementType Type
        { get; private set; }

        public MarkdownText(
            string content
        ) {
            Type = MarkdownElementType.Text;
            this.content = ReplaceEscapeCharacters(
                content
            );
        }

        public string ToHtml() {
            return content;
        }

        private string ReplaceEscapeCharacters(
            string text
        ) {
            foreach (string[] replacement in EscapeReplacements)
            {
                text = text.Replace(
                    replacement[0],
                    replacement[1]
                );
            }
            return text;
        }

        // Given a text snippet, parse a plain text section from its start
        public static ParseResult ParseFrom(
            ParseInput input,
            bool force
        ) {
            string line = input.FirstLine;
            ParseResult result = new ParseResult();
            int indexFirstSpecialCharacter = FindUnescapedSpecial(
                line
            );
            // If force, then ensure at least one character is consumed
            if (
                force
                && (indexFirstSpecialCharacter == 0)
            ) {
                indexFirstSpecialCharacter++;
            }
            // If an empty string is `parsed` then fail
            if (indexFirstSpecialCharacter == 0)
            {
                result.Line = line;
                return result;
            } else if (indexFirstSpecialCharacter != line.Length)
            {
                // There is some special character in the string
                // Add as plain text only the content up to where it starts
                MarkdownText element = new MarkdownText(
                    line.Substring(0, indexFirstSpecialCharacter)
                );
                result.AddContent(element);
                result.Line = line.Substring(indexFirstSpecialCharacter);
                result.Success = true;
            } else {
                // If there are no special sections, everything is plain text
                MarkdownText element = new MarkdownText(
                    line
                );
                result.AddContent(element);
                result.Line = "";
                result.Success = true;
            }
            return result;
        }

        /* 
         * Finds the index of the first unescaped star in the provided string
         * Returns the string string length if none can be found
         */
        private static int FindUnescapedSpecial(
            string line
        ) {
            // First char is special case, cannot be escaped
            if (
                IsInArray(
                        line[0],
                        specialCharacters
                    )
            ) {
                return 0;
            }
            int j = 1;
            while (
                (j < line.Length)
                && !(
                    IsInArray(
                        line[j],
                        specialCharacters
                    )
                    && (line[j-1] != '\\')
                )
            ) {
                j++;
            }
            return j;
        }

        // Check whether a value is in an array
        private static bool IsInArray<T>(
            T value,
            T[] array
        ) {
            return Array.Exists(
                array,
                element => element.Equals(value)
            );
        }

    }
}