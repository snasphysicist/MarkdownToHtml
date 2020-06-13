
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class Strikethrough : IMarkdownParser
    {
        private static Regex regexStruckthroughText = new Regex(
            @"^~{2}(.+)~{2}.*"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            return regexStruckthroughText.Match(input[0].Text).Success;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            string line = input[0].Text;
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
            input[0].Text = regexStruckthroughText.Replace(
                line,
                ""
            );
            result.Success = true;
            return result;
        }
    }
}