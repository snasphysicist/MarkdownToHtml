
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("a")]
        [DataRow("abbr")]
        [DataRow("acronym")]
        [DataRow("audio")]
        [DataRow("b")]
        [DataRow("bdi")]
        [DataRow("bdo")]
        [DataRow("big")]
        [DataRow("br")]
        [DataRow("button")]
        [DataRow("canvas")]
        [DataRow("cite")]
        [DataRow("code")]
        [DataRow("data")]
        [DataRow("datalist")]
        [DataRow("del")]
        [DataRow("dfn")]
        [DataRow("em")]
        [DataRow("embed")]
        [DataRow("i")]
        [DataRow("iframe")]
        [DataRow("img")]
        [DataRow("input")]
        [DataRow("ins")]
        [DataRow("kbd")]
        [DataRow("label")]
        [DataRow("map")]
        [DataRow("mark")]
        [DataRow("meter")]
        [DataRow("noscript")]
        [DataRow("object")]
        [DataRow("output")]
        [DataRow("picture")]
        [DataRow("progress")]
        [DataRow("q")]
        [DataRow("ruby")]
        [DataRow("s")]
        [DataRow("samp")]
        [DataRow("script")]
        [DataRow("select")]
        [DataRow("slot")]
        [DataRow("small")]
        [DataRow("span")]
        [DataRow("strong")]
        [DataRow("sub")]
        [DataRow("sup")]
        [DataRow("svg")]
        [DataRow("template")]
        [DataRow("textarea")]
        [DataRow("time")]
        [DataRow("u")]
        [DataRow("tt")]
        [DataRow("var")]
        [DataRow("video")]
        [DataRow("wbr")]
        public void ElementsDefinedAsInlineInHtmlSpecificationRecognisedAsInlineType(
            string element
        ) {
            HtmlTagName tag = new HtmlTagName(
                element
            );
            Assert.AreEqual(
                HtmlDisplayType.Inline,
                tag.Type
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("fhudisj")]
        [DataRow("k")]
        [DataRow("ufds78534hnu")]
        [DataRow("error")]
        [DataRow("human")]
        public void ElementsNotDefinedInHtmlSpecificationRecognisedAsInlineType(
            string element
        ) {
            HtmlTagName tag = new HtmlTagName(
                element
            );
            Assert.AreEqual(
                HtmlDisplayType.Inline,
                tag.Type
            );
        }
    }
}