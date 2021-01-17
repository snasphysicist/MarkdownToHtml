
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
        public void NonLineBreakingWhitespaceAfterTagOpenerIsNotValidHtmlTag()
        {
            HtmlToken[] tokens = new HtmlToken[]
            {
                new HtmlToken(
                    HtmlTokenType.LessThan,
                    "<"
                ),
                new HtmlToken(
                    HtmlTokenType.NonLineBreakingWhitespace,
                    " "
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
            Assert.IsFalse(HtmlTag.IsValidTag(tokens));
        }

        [TestMethod]
        [Timeout(500)]
        public void LessThanTextNonLineBreakingWhitespaceGreaterThanSequenceIsAValidHtmlTag()
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
                    HtmlTokenType.NonLineBreakingWhitespace,
                    "\t"
                ),
                new HtmlToken(
                    HtmlTokenType.NonLineBreakingWhitespace,
                    "   "
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
        public void SeriesOfTokensRepresentingHtmlOpeningTagWithAttributesIsAValidHtmlTag()
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
                    HtmlTokenType.NonLineBreakingWhitespace,
                    "  "
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
                    "a"
                ),
                new HtmlToken(
                    HtmlTokenType.Text,
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
                    HtmlTokenType.Text,
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
            Assert.IsTrue(HtmlTag.IsValidTag(tokens));
        }
    }
}