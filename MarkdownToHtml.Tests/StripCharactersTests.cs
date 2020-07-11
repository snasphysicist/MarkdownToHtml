
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class StripCharactersTests
    {
        [TestMethod]
        [Timeout(500)]
        public void StripLeadingCharactersWithoutLimitFromEmptyStringReturnsEmptyString() 
        {
            string original = "";
            char toStrip = ' ';
            string stripped = original.StripLeadingCharacters(toStrip);
            Assert.AreEqual(original, stripped);
        }

        [TestMethod]
        [Timeout(500)]
        public void StripLeadingCharactersWithLimitFromEmptyStringReturnsEmptyString() 
        {
            string original = "";
            char toStrip = ' ';
            int limit = 10;
            string stripped = original.StripLeadingCharacters(toStrip, limit);
            Assert.AreEqual(original, stripped);
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("   ", "", ' ')]
        [DataRow("   test", "test", ' ')]
        public void StripLeadingCharactersWithoutLimitRemovesAllInstanceFromStart(
            string original,
            string expected,
            char toStrip
        ) {   
            string stripped = original.StripLeadingCharacters(toStrip);
            Assert.AreEqual(expected, stripped);
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("    ", "", ' ', 5)]
        [DataRow("    ", "  ", ' ', 2)]
        [DataRow("   test", "  test", ' ', 1)]
        [DataRow("   test", "test", ' ', 10)]
        public void StripLeadingCharactersWithLimitRemovesNoMoreCharactersThanLimit(
            string original,
            string expected,
            char toStrip,
            int limit
        ) {   
            string stripped = original.StripLeadingCharacters(toStrip, limit);
            Assert.AreEqual(expected, stripped);
        }

        [TestMethod]
        [Timeout(500)]
        public void StripLeadingCharactersWithoutLimitOnlyRemovesCharactersAtStart() 
        {
            string original = " t e s t ";
            string expected = "t e s t ";
            char toStrip = ' ';
            string stripped = original.StripLeadingCharacters(toStrip);
            Assert.AreEqual(expected, stripped);
        }

        [TestMethod]
        [Timeout(500)]
        public void StripLeadingCharactersWithLimitOnlyRemovesCharactersFromStart() 
        {
            string original = " t e s t ";
            string expected = "t e s t ";
            char toStrip = ' ';
            int limit = 10;
            string stripped = original.StripLeadingCharacters(toStrip, limit);
            Assert.AreEqual(expected, stripped);
        }

        [TestMethod]
        [Timeout(500)]
        public void StripTrailingCharactersOnEmptyStringReturnsEmptyString()
        {
            string original = "";
            char toStrip = ' ';
            string stripped = original.StripTrailingCharacters(toStrip);
            Assert.AreEqual(original, stripped);
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("test", "test", ' ')]
        [DataRow("test   ", "test", ' ')]
        public void StripTrailingCharactersRemovesAllCharactersFromEnd(
            string orignal,
            string expected,
            char toStrip
        )
        {
            string stripped = original.StripTrailingCharacters(toStrip);
            Assert.AreEqual(expected, stripped);
        }

        [TestMethod]
        [Timeout(500)]
        public void StripTrailingCharactersOnlyRemovesCharactersFromEnd()
        {
            string original = " t e s t    ";
            string expected = " t e s t";
            char toStrip = ' ';
            string stripped = original.StripTrailingCharacters(toStrip);
            Assert.AreEqual(expected, stripped);
        }
    }
}
