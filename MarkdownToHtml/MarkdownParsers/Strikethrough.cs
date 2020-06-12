
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class Strikethrough
    {
        private static Regex regexStruckthroughText = new Regex(
            @"^~{2}(.+)~{2}.*"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            return regexStruckthroughText.Match(input.FirstLine).Success;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            string line = input.FirstLine;
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                // Fail immediately if we cannot parse this text as strikethrough
                return result;
            }
            Match contentMatch = regexStruckthroughText.Match(
                line
            );
            string innerText = contentMatch.Groups[1].Value;
            // Parse everything inside the stars
            Element strikethrough = new ElementFactory().New(
                ElementType.Strikethrough,
                MarkdownParser.ParseInnerText(
                    new ParseInput(
                        input,
                        innerText
                    )
                )
            );
            result.AddContent(
                strikethrough
            );
            input.FirstLine = regexStruckthroughText.Replace(
                line,
                ""
            );
            result.Success = true;
            return result;
        }
    }
}