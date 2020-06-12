
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class PreformattedCodeBlock : IMarkdownParser
    {
        private static MarkdownText newLine = new MarkdownText(
            "\n"
        );

        private static Regex regexIndentedLineStart = new Regex(
            @"^ {4}.*"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            return regexIndentedLineStart.Match(input.FirstLine).Success;
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
            int endOfCodeBlock = Utils.FindEndOfSection(
                lines,
                "    "
            );
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            for (int i = 0; i < endOfCodeBlock; i++)
            {
                string line = lines[i];
                // Remove four leading spaces, if present
                if (line.Length > 3)
                {
                    line = line.Substring(4);
                }
                innerContent.AddLast(
                    new MarkdownText(
                        line
                    )
                );
                // Add a newline character after all lines except final line
                if (i != endOfCodeBlock - 1)
                {
                    innerContent.AddLast(
                        newLine
                    );
                }
                // Clear original line from original data
                lines[i] = "";
            }
            Element codeBlock = new ElementFactory().New(
                ElementType.CodeBlock,
                Utils.LinkedListToArray(innerContent)
            );
            Element preformatted = new ElementFactory().New(
                ElementType.Preformatted,
                codeBlock
            );
            result.Success = true;
            result.AddContent(preformatted);
            return result;
        }
    }
}