
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlTagTypeDetectionTests
    {
        [TestMethod]
        [Timeout(500)]
        public void OpeningTagWithoutAttributesIsOfOpeningType()
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
                HtmlTagType.Opening,
                detected.Tag.Type
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void OpeningTagWithAttributesIsOfOpeningType()
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
                HtmlTagType.Opening,
                detected.Tag.Type
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void ClosingTagIsOfClosingType()
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
                HtmlTagType.Closing,
                detected.Tag.Type
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void SelfClosingTagWithoutAttributesIsOfSelfClosingType()
        {
            HtmlToken[] tokens = new HtmlToken[]
            {
                new HtmlToken(
                    HtmlTokenType.LessThan,
                    "<"
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "div"
                ),
                new HtmlToken(
                    HtmlTokenType.ForwardSlash,
                    "/"
                ),
                new HtmlToken(
                    HtmlTokenType.GreaterThan,
                    ">"
                )
            };
            HtmlTagDetector detector = new HtmlTagDetector(tokens);
            HtmlSnippet detected = detector.Detect();
            Assert.AreEqual(
                HtmlTagType.SelfClosing,
                detected.Tag.Type
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void SelfClosingTagWithAttributesIsOfSelfClosingType()
        {
            HtmlToken[] tokens = new HtmlToken[]
            {
                new HtmlToken(
                    HtmlTokenType.LessThan,
                    "<"
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "img"
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
                    HtmlTokenType.ForwardSlash,
                    "/"
                ),
                new HtmlToken(
                    HtmlTokenType.GreaterThan,
                    ">"
                )
            };
            HtmlTagDetector detector = new HtmlTagDetector(tokens);
            HtmlSnippet detected = detector.Detect();
            Assert.AreEqual(
                HtmlTagType.SelfClosing,
                detected.Tag.Type
            );
        }
    }
}
