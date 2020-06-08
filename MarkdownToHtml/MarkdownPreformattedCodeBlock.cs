
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownPreformattedCodeBlock : MarkdownElementWithContent, IHtmlable
    {
        private static MarkdownText newLine = new MarkdownText(
            "\n"
        );

        private static Regex regexIndentedLineStart = new Regex(
            @"^ {4}.*"
        );

        public MarkdownPreformattedCodeBlock(
            IHtmlable[] content
        ) {
            Type = MarkdownElementType.CodeBlock;
            this.content = content;
        }

        public static bool CanParseFrom(
            ParseInput input
        ) {
            return regexIndentedLineStart.Match(input.FirstLine).Success;
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
            MarkdownPreformatted codeBlock = new MarkdownPreformatted(
                new MarkdownCodeBlock(
                    Utils.LinkedListToArray(innerContent)
                )
            );
            result.Success = true;
            result.AddContent(codeBlock);
            return result;
        }

        class MarkdownPreformatted : MarkdownElementWithContent, IHtmlable
        {
            public MarkdownPreformatted(
                IHtmlable content
            ) {
                Type = MarkdownElementType.Preformatted;
                this.content = new IHtmlable[]
                {
                    content
                };
            }
        }

    }
}
