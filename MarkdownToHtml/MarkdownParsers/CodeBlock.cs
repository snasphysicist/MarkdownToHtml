
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class CodeBlock : IMarkdownParser
    {
        private static Regex regexBacktickSectionOpen = new Regex(
            @"^`{3}.*"
        );
        private static Regex regexBacktickSectionClose = new Regex(
            @"^`{3}"
        );

        public bool CanParseFrom(
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

        public ParseResult ParseFrom(
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
            Element element = new ElementFactory().New(
                ElementType.CodeBlock,
                Utils.LinkedListToArray(innerContent)
            );
            result.Success = true;
            result.AddContent(element);
            return result;
        }
    }
}