
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class MarkdownParagraph : IHtmlable
    {

        IHtmlable[] content;

        const string tag = "p";

        public const MarkdownElementType Type = MarkdownElementType.Paragraph;

        public MarkdownParagraph(
            IHtmlable[] innerContent
        ) {
            content = innerContent;
        }

        public string ToHtml() 
        {
            string html = $"<{tag}>";
            foreach (IHtmlable htmlable in content)
            {
                html += htmlable.ToHtml();
            }
            html += $"</{tag}>";
            return html;
        }

        // Parse a plain paragraph
        public static ParseResult ParseFrom(
            ArraySegment<string> lines
        ) {
            ParseResult result = new ParseResult();
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                if (endsWithAtLeastTwoSpaces(line))
                {
                    string shortened = StripTrailingWhitespace(line);
                    foreach (IHtmlable entry in MarkdownParser.ParseInnerText(shortened))
                    {
                        innerContent.AddLast(entry);
                    }
                    innerContent.AddLast(
                        new MarkdownLinebreak()
                    );
                } else {
                    foreach (IHtmlable entry in MarkdownParser.ParseInnerText(line))
                    {
                        innerContent.AddLast(entry);
                    }
                    /*
                     * If this is not the last line,
                     * it doesn't end in a manual linebreak
                     * and the user hasn't added a space themselves
                     * we need to add a space at the end
                     */
                    if (
                        (i != (lines.Count - 1))
                        && (line.Length > 0)
                        && (line[^1] != ' ')
                    ) {
                        innerContent.AddLast(
                            new MarkdownText(" ")
                        );
                    }
                }
            }
            MarkdownParagraph paragraph = new MarkdownParagraph(
                LinkedListToArray(innerContent)
            );
            result.Success = true;
            result.AddContent(
                paragraph
            );
            return result;
        }

        private static bool endsWithAtLeastTwoSpaces (
            string line
        ) {
            if (line.Length > 1)
            {
                return line.Substring(
                    line.Length - 2,
                    2
                ) == "  ";
            } else
            {
                return false;
            }
        }

        private static string StripTrailingWhitespace(
            string line
        ) {
            return StripTrailingCharacter(
                line,
                ' '
            );
        }

        private static string StripTrailingCharacter(
            string line,
            char character
        ) {
            while (
                (line.Length > 0)
                && (line[^1] == character)
            ) {
                line = line.Substring(
                    0,
                    line.Length - 1 
                );
            }
            return line;
        }

        private static T[] LinkedListToArray<T>(
            LinkedList<T> linkedList
        ) {
            T[] array = new T[linkedList.Count];
            linkedList.CopyTo(array, 0);
            return array;
        }

    }
}