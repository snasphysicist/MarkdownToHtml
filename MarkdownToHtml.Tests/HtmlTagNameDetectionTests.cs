
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlTagNameDetectionTests
    {
        [TestMethod]
        [Timeout(500)]
        public void TextImmediatelyFollowingOpenerInOpeningTagWithoutAttributesIsTheTagName()
        {
            HtmlToken[] tokens = new HtmlToken[]
            {
                new HtmlToken(
                    HtmlTokenType.LessThan,
                    "<"
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "p"
                ),
                new HtmlToken(
                    HtmlTokenType.GreaterThan,
                    ">"
                )
            };
            HtmlTagDetector detector = new HtmlTagDetector(tokens);
            HtmlSnippet detected = detector.Detect();
            Assert.AreEqual(
                detected.Tag.Name.Name,
                "p"
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void TextDirectlyAfterOpenerIsTheTagNameForAnOpeningTagWithAttributes()
        {
            HtmlToken[] tokens = new HtmlToken[]
            {
                new HtmlToken(
                    HtmlTokenType.LessThan,
                    "<"
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "help"
                ),
                new HtmlToken(
                    HtmlTokenType.NonLineBreakingWhitespace,
                    "  "
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "a"
                ),
                new HtmlToken(
                    HtmlTokenType.Equals,
                    "="
                ),
                new HtmlToken(
                    HtmlTokenType.DoubleQuote,
                    "\""
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "yhtvc754"
                ),
                new HtmlToken(
                    HtmlTokenType.DoubleQuote,
                    "\""
                ),
                new HtmlToken(
                    HtmlTokenType.NonLineBreakingWhitespace,
                    "\t\t\t"
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "ctvyh745"
                ),
                new HtmlToken(
                    HtmlTokenType.Equals,
                    "="
                ),
                new HtmlToken(
                    HtmlTokenType.DoubleQuote,
                    "\""
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "3498-vnt58y7"
                ),
                new HtmlToken(
                    HtmlTokenType.DoubleQuote,
                    "\""
                ),
                new HtmlToken(
                    HtmlTokenType.GreaterThan,
                    ">"
                )
            };
            HtmlTagDetector detector = new HtmlTagDetector(tokens);
            HtmlSnippet detected = detector.Detect();
            Assert.AreEqual(
                "help",
                detected.Tag.Name.Name
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void TextImmediatelyFollowingForwardSlashIsTagNameForClosingTag()
        {
            HtmlToken[] tokens = new HtmlToken[]
            {
                new HtmlToken(
                    HtmlTokenType.LessThan,
                    "<"
                ),
                new HtmlToken(
                    HtmlTokenType.ForwardSlash,
                    "/"
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "video"
                ),
                new HtmlToken(
                    HtmlTokenType.GreaterThan,
                    ">"
                )
            };
            HtmlTagDetector detector = new HtmlTagDetector(tokens);
            HtmlSnippet detected = detector.Detect();
            Assert.AreEqual(
                "video",
                detected.Tag.Name.Name
            );
        }
    }
}
