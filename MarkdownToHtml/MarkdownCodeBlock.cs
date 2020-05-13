
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownCodeBlock : IHtmlable
    {

        private static Regex regexBacktickSectionOpen = new Regex(
            @"^`{3}.*"
        );

        private static Regex regexBacktickSectionClose = new Regex(
            @"^`{3}"
        );

        IHtmlable[] content;

        const string tag = "code";

        public const MarkdownElementType Type = MarkdownElementType.CodeInline;

        public MarkdownCodeBlock(
            IHtmlable[] content
        ) {
            this.content = content;
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

        public static bool CanParseFrom(
            ArraySegment<string> lines
        ) {
            if (!regexBacktickSectionOpen.Match(lines[0]).Success)
            {
                return false;
            } else {
                for (int i = 1; i < lines.Count; i++)
                {
                    if (regexBacktickSectionClose.Match(lines[i]).Success)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static ParseResult ParseFrom(
            ArraySegment<string> lines
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(lines))
            {
                return result;
            }
            lines[0] = "";
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            int i = 1;
            while (!regexBacktickSectionClose.Match(lines[i]).Success)
            {
                innerContent.AddLast(
                    new MarkdownText(
                        lines[i]
                    )
                );
                lines[i] = "";
                i++;
            }
            MarkdownParagraph blockCodeElement = new MarkdownParagraph(
                new IHtmlable[] {
                    new MarkdownCodeBlock(
                        LinkedListToArray(innerContent)
                    )
                }
            );
            result.Success = true;
            result.AddContent(blockCodeElement);
            return result;
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
