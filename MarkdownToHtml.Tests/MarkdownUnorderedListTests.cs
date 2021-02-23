
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownUnorderedListTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("* test1", "<ul><li>test1</li>\n</ul>\n")]
        [DataRow("+ test1", "<ul><li>test1</li>\n</ul>\n")]
        [DataRow("- test1", "<ul><li>test1</li>\n</ul>\n")]
        public void ShouldParseUnorderedListStarPlusMinusSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(" * test1", "<ul><li>test1</li>\n</ul>\n")]
        [DataRow("  * test1", "<ul><li>test1</li>\n</ul>\n")]
        [DataRow("   * test1", "<ul><li>test1</li>\n</ul>\n")]
        public void ShouldParseUnorderedListOneToThreeSpacesSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("* test1\n* test2", "<ul><li>test1</li>\n<li>test2</li>\n</ul>\n")]
        public void ShouldParseUnorderedListLinesNotSeparatedByWhitespaceSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(
            "* test1\n+ test2\n- test3", 
            "<ul><li>test1</li>\n<li>test2</li>\n<li>test3</li>\n</ul>\n"
        )]
        public void ShouldParseUnorderedListDifferentSymbolsAsOneListSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(
            "* test1\n\n\n* test2", 
            "<ul><li><p>test1</p>\n</li>\n<li><p>test2</p>\n</li>\n</ul>\n"
        )]
        public void ShouldParseUnorderedListLinesSeparatedByWhitespaceSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(
            "* test1\n\ntest2\n\n* test3", 
            "<ul><li>test1</li>\n</ul>\n<p>test2</p>\n<ul><li>test3</li>\n</ul>\n"
        )]
        public void ShouldParseUnorderedListLinesSeparatedByNormalParagraphSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(
            "* test1\ntest2\n\n* test3", 
            "<ul><li><p>test1 test2</p>\n</li>\n<li><p>test3</p>\n</li>\n</ul>\n"
        )]
        public void ShouldParseUnorderedListLineAfterParagraphAsParagraphSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(
            "* test1\n1. test2", 
            "<ul><li>test1</li>\n<li>test2</li>\n</ul>\n"
        )]
        public void ShouldParseStartsWithStarMixedOrderedUnorderedAsUnorderedSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("*test1", "<p>*test1</p>\n")]
        public void ShouldParseImproperlyFormattedUnorderedListAsParagraphSuccess(
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

        [TestMethod]
        [Timeout(500)]
        public void ListItemEndingInInlineCodeElementIsNotWrappedInAParagraphIfItIsNotAdjacentToAWhitespaceLine()
        {
            string markdown = "- UART0\n    - Configuration 1, byte value `0x01`\n    - Configuration 2, byte value `0x02`";
            string html = 
                "<ul><li>UART0<ul><li>Configuration 1, byte value <code>0x01</code></li>\n" + 
                "<li>Configuration 2, byte value <code>0x02</code></li>\n</ul>\n</li>\n</ul>\n";
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assertions.AssertStringsEqualShowingDifference(
                html,
                parser.ToHtml()
            );
        }
    }
}