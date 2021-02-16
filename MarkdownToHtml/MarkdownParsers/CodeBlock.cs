
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
            if (!regexBacktickSectionOpen.Match(input[0].Text).Success)
            {
                return false;
            } else {
                for (int i = 1; i < input.Count; i++)
                {
                    if (regexBacktickSectionClose.Match(input[i].Text).Success)
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
            input[0].WasParsed();
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            int i = 1;
            while (!regexBacktickSectionClose.Match(input[i].Text).Success)
            {
                innerContent.AddLast(
                    MarkdownText.NotEscapingReplacedHtml(
                        input[i].Text
                    )
                );
                input[i].WasParsed();
                i++;
            }
            // Remember to clear final line (closing backticks)
            input[i].WasParsed();
            Element element = new ElementFactory().New(
                ElementType.Paragraph,
                new ElementFactory().New(
                    ElementType.CodeBlock,
                    innerContent.ToArray()
                )
            );
            result.Success = true;
            result.AddContent(element);
            return result;
        }
    }
}