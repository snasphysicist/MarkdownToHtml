
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
    }
}


