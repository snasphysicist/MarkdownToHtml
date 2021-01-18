
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
                HtmlTagDetector detector = new HtmlTagDetector(tagTokens);
                HtmlSnippet detected = detector.Detect();
                Assert.IsTrue(detected.IsToken());
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
                HtmlTagDetector detector = new HtmlTagDetector(tokens);
                HtmlSnippet detected = detector.Detect();
                Assert.IsTrue(detected.IsTag());
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
                HtmlTagDetector detector = new HtmlTagDetector(tokens);
                HtmlSnippet detected = detector.Detect();
                Assert.IsTrue(detected.IsToken());
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
                HtmlTagDetector detector = new HtmlTagDetector(tokens);
                HtmlSnippet detected = detector.Detect();
                Assert.IsTrue(detected.IsTag());
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
                Assert.IsTrue(detected.IsTag());
        }

        [TestMethod]
        [Timeout(500)]
        public void LessThanForwardSlashTextGreaterThanSequenceIsAValidHtmlTag()
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
                    "p"
                ),
                new HtmlToken(
                    HtmlTokenType.GreaterThan,
                    ">"
                )
            };
            HtmlTagDetector detector = new HtmlTagDetector(tokens);
            HtmlSnippet detected = detector.Detect();
            Assert.IsTrue(detected.IsTag());
        }

        [TestMethod]
        [Timeout(500)]
        public void ClosingTagIncludingNonLineBreakingWhitespaceAfterTagNameIsAValidHtmlTag()
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
                    "p"
                ),
                new HtmlToken(
                    HtmlTokenType.NonLineBreakingWhitespace,
                    "   "
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
            HtmlTagDetector detector = new HtmlTagDetector(tokens);
            HtmlSnippet detected = detector.Detect();
            Assert.IsTrue(detected.IsTag());
        }

        [TestMethod]
        [Timeout(500)]
        public void ClosingTagIncludingLineBreakingWhitespaceAfterTagNameIsNotAValidHtmlTag()
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
                    "p"
                ),
                new HtmlToken(
                    HtmlTokenType.LineBreakingWhitespace,
                    "\n"
                ),
                new HtmlToken(
                    HtmlTokenType.GreaterThan,
                    ">"
                )
            };
            HtmlTagDetector detector = new HtmlTagDetector(tokens);
            HtmlSnippet detected = detector.Detect();
            Assert.IsFalse(detected.IsTag());
        }

        [TestMethod]
        [Timeout(500)]
        public void LessThanTextForwardSlashGreaterThanSequenceIsAValidHtmlTag()
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
            Assert.IsTrue(detected.IsTag());
        }

        [TestMethod]
        [Timeout(500)]
        public void SelfClosingTagWithLeadingNonLineBreakingWhitespaceIsNotAValidHtmlTag()
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
            Assert.IsFalse(detected.IsTag());
        }
    }
}