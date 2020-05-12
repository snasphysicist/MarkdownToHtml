
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class MarkdownBlockCodeTests
    {

        [DataTestMethod]
        [DataRow("```\ntest1\n```", "<p><code>test1</code></p>")]
        [DataRow("test1\n```\ntest2\n```", "<p>test1<code>test2</code></p>")]
        [DataRow("test1\n\n```\ntest2\n```", "<p>test1</p><p><code>test2</code></p>")]
        public void ShouldParseProperlyDelimitedBacktickCodeBlockSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split('\n')
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
        [DataRow("```\ntest1\n\ntest2", "<p>``` test1</p><p>test2</p>")]
        public void ShouldParseImproperlyDelimitedBacktickCodeBlockAsParagraphSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split('\n')
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

    }
}