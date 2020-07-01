
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
            return regexIndentedLineStart.Match(input[0].Text).Success;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {

            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            int endOfCodeBlock = Utils.FindEndOfSection(
                input,
                "    "
            );
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            for (int i = 0; i < endOfCodeBlock; i++)
            {
                string line = input[i].Text;
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
                input[i].WasParsed();
            }
            Element codeBlock = new ElementFactory().New(
                ElementType.CodeBlock,
                innerContent.ToArray()
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