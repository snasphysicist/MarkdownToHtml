
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlTagTests
    {
        [TestMethod]
        [Timeout(500)]
        public void ASingleTokenIsNotAValidHtmlTag()
        {
            HtmlToken[] tokens = new HtmlToken[]
            {
                new HtmlToken(
                    HtmlTokenType.Text,
                    "nhjfgriled"
                ),
                new HtmlToken(
                    HtmlTokenType.LineBreakingWhitespace,
                    "\n"
                ),
                new HtmlToken(
                    HtmlTokenType.NonLineBreakingWhitespace,
                    "\t"
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
                ),
                new HtmlToken(
                    HtmlTokenType.LessThan,
                    "<"
                )
            };
            foreach (HtmlToken token in tokens)
            {
                HtmlToken[] tagTokens = new HtmlToken[]
                {
                    token
                };
                Assert.IsFalse(HtmlTag.IsValidTag(tagTokens));
                Assert.ThrowsException<ArgumentException>(() => HtmlTag.FromTokens(tagTokens));
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void LessThanTextGreaterThanSequenceIsAValidHtmlTag()
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
            Assert.IsTrue(HtmlTag.IsValidTag(tokens));
        }

        [TestMethod]
        [Timeout(500)]
        public void LessThanNonLineBreakingWhitespaceTextNonLineBreakingWhitespaceGreaterThanSequenceIsAValidHtmlTag()
        {
            HtmlToken[] tokens = new HtmlToken[]
            {
                new HtmlToken(
                    HtmlTokenType.LessThan,
                    "<"
                ),
                new HtmlToken(
                    HtmlTokenType.NonLineBreakingWhitespace,
                    "   "
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "p"
                ),
                new HtmlToken(
                    HtmlTokenType.NonLineBreakingWhitespace,
                    "\t"
                ),
                new HtmlToken(
                    HtmlTokenType.GreaterThan,
                    ">"
                )
            };
            Assert.IsTrue(HtmlTag.IsValidTag(tokens));
        }
    }
}