
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class MarkdownStrongEmphasisTests
    {
        [DataTestMethod]
        [Timeout(500)]
        [DataRow("***test***", "<strong><em>test</em></strong>")]
        [DataRow("Some ___other___ text", "<strong><em>other</em></strong>")]
        public void ThreeUnescapedStarsOrUnderscoresAreParsedAsStrongEmphasis(
            string markdown,
            string html
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
        }
    }
}