
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownBlockquoteTests
    {
        [DataTestMethod]
        [Timeout(500)]
        [DataRow(">test", "<blockquote><p>test</p>\n</blockquote>")]
        [DataRow("> test", "<blockquote><p>test</p>\n</blockquote>")]
        [DataRow(">  test", "<blockquote><p>test</p>\n</blockquote>")]
        [DataRow(">   test", "<blockquote><p>test</p>\n</blockquote>")]
        [DataRow(">    test", "<blockquote><p>test</p>\n</blockquote>")]
        public void ShouldParseBlockquoteZeroToFourSpacesSuccessfully(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }
        
    }
}
