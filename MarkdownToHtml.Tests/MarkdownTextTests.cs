
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class MarkdownTextTests
    {
        [TestMethod]
        [Timeout(500)]
        public void HtmlSpecialCharactersInTextAreReplaced()
        {
            string input = "& < > ' \"";
            string expectedOutput = "&amp; &lt; &gt; &apos; &quot;";
            MarkdownText text = new MarkdownText(
                input
            );
            Assert.AreEqual(
                expectedOutput,
                text.ToHtml()
            );
        }
    }
}