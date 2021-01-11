
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MarkdownToHtml 
{
    [TestClass]
    public class HtmlTokeniserTests
    {
        [DataTestMethod]
        [Timeout(500)]
        [DataRow("jdf394AHk29MSD")]
        public void GroupOfContiguousNonWhitespaceCharactersExcludingKeyHtmlCharactersAreATextToken(
            string content
        ) {
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            LinkedList<HtmlToken> tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Count
            );
            HtmlToken token = tokens.First.Value;
            Assert.AreEqual(
                HtmlTokenType.Text,
                token.Type
            );
        }
    }
}
