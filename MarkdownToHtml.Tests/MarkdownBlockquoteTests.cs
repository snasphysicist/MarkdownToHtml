
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownBlockquoteTests
    {
        [DataTestMethod]
        [DataRow(">test", "<blockquote><p>test</p></blockquote>")]
        [DataRow("> test", "<blockquote><p>test</p></blockquote>")]
        [DataRow(">  test", "<blockquote><p>test</p></blockquote>")]
        [DataRow(">   test", "<blockquote><p>test</p></blockquote>")]
        public void ShouldParseBlockquoteZeroToThreeSpacesSuccessfully(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                new string[] 
                {
                    markdown
                }
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
