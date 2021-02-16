
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownText : IHtmlable
    {

        // Special characters that could start different element type
        private static Regex regexSpecialCharacter = new Regex(
            @"[^\\]([`|\*|_|\[|\]|#|!|~])"
        );
        
        private static char[] specialCharacters = new char[]
        {
            '\\',
            '`',
            '*',
            '_',
            '{',
            '}',
            '[',
            ']',
            '(',
            ')',
            '#',
            '+',
            '-',
            '.',
            '!',
            '~'
        };

        private HtmlElementInserter reinsertedContent;

        public ElementType Type
        { get; private set; }

        public static MarkdownText NotEscapingReplacedHtml(
            string content
        ) {
            return new MarkdownText(
                new HtmlElementInserter(
                    new Dictionary<Guid, string>(),
                    content
                )
            );
        }

        public static MarkdownText EscapingReplacedHtml(
            string content,
            Dictionary<Guid, string> replacements
        ) {
            return new MarkdownText(
                new HtmlElementInserter(
                    replacements, 
                    content
                )
            );
        }

        private MarkdownText(
            HtmlElementInserter inserter
        ) {
            Type = ElementType.Text;
            this.reinsertedContent = inserter;
        }

        public string ToHtml() {
            HtmlSpecialCharacterEscaper escaper = new HtmlSpecialCharacterEscaper(
                reinsertedContent.Processed
            );
            return ReplaceEscapeCharacters(
                escaper.Escaped
            );
        }

        private string ReplaceEscapeCharacters(
            string text
        ) {
            foreach (char special in specialCharacters)
            {
                // escapeSequence like \\# -> replace with last char (#)
                text = text.Replace(
                    new string(
                        new char[]{
                            '\\', 
                            special
                        }
                    ),
                    new string(special, 1)
                );
            }
            return text;
        }

        // Given a text snippet, parse a plain text section from its start
        public static ParseResult ParseFrom(
            ParseInput input,
            bool force
        ) {
            string line = input[0].Text;
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
                return result;
            } else if (indexFirstSpecialCharacter != line.Length)
            {
                // There is some special character in the string
                // Add as plain text only the content up to where it starts
                MarkdownText element = MarkdownText.NotEscapingReplacedHtml(
                    line.Substring(0, indexFirstSpecialCharacter)
                );
                result.AddContent(element);
                input[0].Text = line.Substring(indexFirstSpecialCharacter);
                result.Success = true;
            } else {
                // If there are no special sections, everything is plain text
                MarkdownText element = MarkdownText.NotEscapingReplacedHtml(
                    line
                );
                result.AddContent(element);
                input[0].WasParsed();
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
            // First character is special case, cannot be escaped
            if (
                IsInArray(
                    line[0],
                    specialCharacters
                )
            ) {
                return 0;
            }
            Match matchSpecialCharacters = regexSpecialCharacter.Match(line);
            if (matchSpecialCharacters.Success)
            {
                // Return index of first capture
                return matchSpecialCharacters.Groups[1].Index;
            } else {
                // No match, return index outside of string
                return line.Length;
            }
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