
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownCodeBlock : MarkdownElement, IHtmlable
    {

        private static Regex regexBacktickSectionOpen = new Regex(
            @"^`{3}.*"
        );

        private static Regex regexBacktickSectionClose = new Regex(
            @"^`{3}"
        );

        public MarkdownCodeBlock(
            IHtmlable[] content
        ) {
            Type = MarkdownElementType.CodeInline;
            this.content = content;
        }

        public static bool CanParseFrom(
            ParseInput input
        ) {
            ArraySegment<string> lines = input.Lines();
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
            ParseInput input
        ) {

            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            ArraySegment<string> lines = input.Lines();
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
            // Remember to clear final line (closing backticks)
            lines[i] = "";
            MarkdownCodeBlock blockCodeElement = new MarkdownCodeBlock(
                Utils.LinkedListToArray(innerContent)
            );
            result.Success = true;
            result.AddContent(blockCodeElement);
            return result;
        }

    }
}
