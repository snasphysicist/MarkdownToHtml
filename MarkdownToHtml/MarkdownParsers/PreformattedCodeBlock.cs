
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class PreformattedCodeBlock : IMarkdownParser
    {
        int indentationLevel;

        private static MarkdownText newLine = MarkdownText.NotEscapingReplacedHtml(
            "\n"
        );

        private static Regex regexIndentedLineStart = new Regex(
            @"^ {4} *.*"
        );

        public PreformattedCodeBlock(
            int indentationLevel
        ) {
            this.indentationLevel = indentationLevel;
        }

        public bool CanParseFrom(
            ParseInput input
        ) {
            return (
                regexIndentedLineStart.Match(input[0].Text).Success
                && (input[0].IndentationLevel() == indentationLevel + 1)
            );
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {

            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            string spacesAtStart = "";
            while (
                input[0].StartsWith(spacesAtStart + " ")
            ) {
                spacesAtStart += " ";
            }
            int endOfCodeBlock = Utils.FindEndOfSection(
                input,
                spacesAtStart
            );
            LinkedList<IHtmlable> innerContent = new LinkedList<IHtmlable>();
            for (int i = 0; i < endOfCodeBlock; i++)
            {
                string line = input[i].Text;
                // Remove up to 4 + indentation spaces
                int toRemove = 0;
                while (
                    toRemove < line.Length 
                    && toRemove < 4 * (1 + indentationLevel)
                    && line.Substring(toRemove, 1) == " "
                ) {
                    toRemove++;
                }
                line = line.Substring(toRemove);
                innerContent.AddLast(
                    MarkdownText.NotEscapingReplacedHtml(
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