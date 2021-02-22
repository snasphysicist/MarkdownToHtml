
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownLinkTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("[text](url)", "<p><a href=\"url\">text</a></p>\n")]
        public void ShouldParseCorrectlyFormattedLinkAdjacentSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.ToHtml();
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("[text][ref]\n\n\n[ref]: url", "<p><a href=\"url\" title=\"\">text</a></p>\n")]
        [DataRow("[text] [ref]\n\n\n[ref]: url", "<p><a href=\"url\" title=\"\">text</a></p>\n")]
        [DataRow("[text]  [ref]\n\n\n[ref]: url", "<p><a href=\"url\" title=\"\">text</a></p>\n")]
        public void SquareBracketedTextFollowedBySquareBracketedTextPossiblyWithWhitespaceBetweenIsAReferenceStyleLink(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.ToHtml();
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("[text][ref]\n\n\n[ref]: url \"the title\"", "<p><a href=\"url\" title=\"the title\">text</a></p>\n")]
        [DataRow("[text][ref]\n\n\n[ref]: url 'the title'", "<p><a href=\"url\" title=\"the title\">text</a></p>\n")]
        [DataRow("[text][ref]\n\n\n[ref]: url (the title)", "<p><a href=\"url\" title=\"the title\">text</a></p>\n")]
        public void TextAfterReferenceLinkInSingleOrDoubleQuotesOrParenthesesIsTitleText(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.ToHtml();
            Assert.AreEqual(
                targetHtml,
                html
            );
        }


        [DataTestMethod]
        [Timeout(500)]
        [DataRow("[text]\n\n\n[text]: url", "<p><a href=\"url\">text</a></p>\n")]
        public void ShouldParseCorrectlyFormattedLinkSelfReferenceSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.ToHtml();
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("[text]", "<p>[text]</p>\n")]
        public void ShouldParseIncorrectlyFormattedLinkAsParagraphSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.ToHtml();
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(
            "test1 [test2](test3) test4", 
            "<p>test1 <a href=\"test3\">test2</a> test4</p>\n"
        )]
        public void ShouldParseCorrectlyFormattedLinkEmbeddedSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.ToHtml();
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void TextBetweenDoubleQuotesAfterUrlInImmediateLinkIsLinkTitle()
        {
            string markdown = "But my favourite search engine is [Bing](https://bing.com \"The worst search engine, period\")";
            string expectedHtml = "<p>But my favourite search engine is " + 
                "<a href=\"https://bing.com\" title=\"The worst search engine, period\">Bing</a></p>\n";
            MarkdownParser parser = new MarkdownParser(markdown);
            Assert.AreEqual(
                expectedHtml, 
                parser.ToHtml()
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void CorrectlyConstructedImageElementInsideLinkParsedAsHtmlImageElementWrappedInAnchorElement()
        {
            string markdown = "[![Alt](/picture.jpg \"Title\")](https://link.url.za)";
            string expectedHtml = "<p><a href=\"https://link.url.za\"><img src=\"/picture.jpg\" alt=\"Alt\" title=\"Title\"></img></a></p>\n";
            MarkdownParser parser = new MarkdownParser(markdown);
            Assert.AreEqual(
                expectedHtml,
                parser.ToHtml()
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void ImmediateLinkInsideBracketsTerminatesHrefTextAtFirstBracketItEncounters()
        {
            string markdown = "(This might normally [be](www.bing.com) problematic.)";
            string html = "<p>(This might normally <a href=\"www.bing.com\">be</a> problematic.)</p>\n";
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.AreEqual(
                html,
                parser.ToHtml()
            );
        }
    }
}
