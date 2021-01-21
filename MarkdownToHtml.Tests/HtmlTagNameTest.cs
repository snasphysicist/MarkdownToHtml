
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlTagNameTest
    {
        [DataTestMethod]
        [Timeout(500)]
        [DataRow("address")]
        [DataRow("article")]
        [DataRow("aside")]
        [DataRow("blockquote")]
        [DataRow("details")]
        [DataRow("dialog")]
        [DataRow("dd")]
        [DataRow("div")]
        [DataRow("dl")]
        [DataRow("dt")]
        [DataRow("fieldset")]
        [DataRow("figcaption")]
        [DataRow("figure")]
        [DataRow("footer")]
        [DataRow("form")]
        [DataRow("h1")]
        [DataRow("h2")]
        [DataRow("h3")]
        [DataRow("h4")]
        [DataRow("h5")]
        [DataRow("h6")]
        [DataRow("header")]
        [DataRow("hgroup")]
        [DataRow("hr")]
        [DataRow("li")]
        [DataRow("main")]
        [DataRow("nav")]
        [DataRow("ol")]
        [DataRow("p")]
        [DataRow("pre")]
        [DataRow("section")]
        [DataRow("table")]
        [DataRow("ul")]
        public void AllElementsDefinedAsBlockInHtmlSpecificationRecognisedAsBlockType(
            string element
        ) {
            HtmlTagName tag = new HtmlTagName(
                element
            );
            Assert.AreEqual(
                HtmlDisplayType.Block,
                tag.Type
            );
        }
    }

}