
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownUnorderedListTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("* test1", "<ul><li>test1</li></ul>")]
        [DataRow("+ test1", "<ul><li>test1</li></ul>")]
        [DataRow("- test1", "<ul><li>test1</li></ul>")]
        public void ShouldParseUnorderedListStarPlusMinusSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(" * test1", "<ul><li>test1</li></ul>")]
        [DataRow("  * test1", "<ul><li>test1</li></ul>")]
        [DataRow("   * test1", "<ul><li>test1</li></ul>")]
        public void ShouldParseUnorderedListOneToThreeSpacesSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("* test1\n* test2", "<ul><li>test1</li><li>test2</li></ul>")]
        public void ShouldParseUnorderedListLinesNotSeparatedByWhitespaceSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
            "<ul><li>test1</li><li>test2</li><li>test3</li></ul>"
        )]
        public void ShouldParseUnorderedListDifferentSymbolsAsOneListSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
            "* test1\n\n\n*test2", 
            "<ul><li><p>test1</p></li><li><p>test2</p></li></ul>"
        )]
        public void ShouldParseUnorderedListLinesSeparatedByWhitespaceSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
            "<ul><li>test1</li></ul><p>test2</p><ul><li>test3</li></ul>"
        )]
        public void ShouldParseUnorderedListLinesSeparatedByNormalParagraphSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
            "<ul><li><p>test1 test2</p></li><li><p>test3</p></li></ul>"
        )]
        public void ShouldParseUnorderedListLineAfterParagraphAsParagraphSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
            "<ul><li>test1</li><li>test2</li></ul>"
        )]
        public void ShouldParseStartsWithStarMixedOrderedUnorderedAsUnorderedSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
        [DataRow("*test1", "<p>*test1</p>")]
        public void ShouldParseImproperlyFormattedUnorderedListAsParagraphSuccess(
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