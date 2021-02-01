
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlSpecialCharacterEscaperTests
    {
        [TestMethod]
        [Timeout(500)]
        public void NonSpecialCharactersAreNotEscaped()
        {
            string nonSpecials = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^*([]{})\\|;:/?,.=+";
            HtmlSpecialCharacterEscaper escaper = new HtmlSpecialCharacterEscaper(nonSpecials);
            Assert.AreEqual(
                nonSpecials,
                escaper.Escaped
            );
        }
    }
}