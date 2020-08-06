
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class LineTests
    {
        [TestMethod]
        [Timeout(500)]
        public void LineInitialisedWithEmptyStringHasBeenParsed() 
        {
            Line line = new Line(
                ""
            );
            Assert.IsTrue(
                line.HasBeenParsed()
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void LineInitialisedWithNonEmptyStringHasNotBeenParsed()
        {
            Line line = new Line(
                "test"
            );
            Assert.IsFalse(
                line.HasBeenParsed()
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("test", 0)]
        [DataRow(" test", 0)]
        [DataRow("  test", 0)]
        [DataRow("   test", 0)]
        [DataRow("    test", 1)]
        [DataRow("     test", 1)]
        [DataRow("      test", 1)]
        [DataRow("       test", 1)]
        [DataRow("        test", 2)]
        [DataRow("         test", 2)]
        [DataRow("          test", 2)]
        [DataRow("           test", 2)]
        [DataRow("            test", 3)]
        [DataRow("             test", 3)]
        [DataRow("              test", 3)]
        [DataRow("               test", 3)]
        [DataRow("                test", 4)]
        [DataRow("                 test", 4)]
        [DataRow("                  test", 4)]
        [DataRow("                   test", 4)]
        public void IndentationLevelIncreasesByOneEachFourLeadingSpaces(
            string text,
            int expectedIndentationLevel
        ) {
            Line line = new Line(
                text
            );
            Assert.AreEqual(
                expectedIndentationLevel,
                line.IndentationLevel()
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void LineContainsOnlyWhitespaceWhenTextIsAllSpaceCharacters() 
        {
            Line line = new Line(
                "         "
            );
            Assert.IsTrue(
                line.ContainsOnlyWhitespace()
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void LineDoesNotContainOnlyWhitespaceWhenTextIsNotAllSpaceCharacters() 
        {
            Line line = new Line(
                "   t      "
            );
            Assert.IsFalse(
                line.ContainsOnlyWhitespace()
            );
        }
    }
}
