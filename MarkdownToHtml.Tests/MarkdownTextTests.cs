
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    [TestClass]
    public class MarkdownTextTests
    {
        [TestMethod]
        [Timeout(500)]
        public void HtmlSpecialCharactersInTextAreReplaced()
        {
            string input = "& < > ' \"";
            string expectedOutput = "&amp; &lt; &gt; &apos; &quot;";
            MarkdownText text = MarkdownText.NotEscapingReplacedHtml(
                input
            );
            Assert.AreEqual(
                expectedOutput,
                text.ToHtml()
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void GuidReplacedWithAssociatedTextWhenMarkdownTextProvidedWithReplacementTextForThatGuid()
        {
            Guid toReplace = Guid.NewGuid();
            string replacement = "REPLACED";
            Dictionary<Guid, string> replacements = new Dictionary<Guid, string>
            {
                {toReplace, replacement}
            };
            MarkdownText text = MarkdownText.EscapingReplacedHtml(
                toReplace.ToString(),
                replacements
            );
            Assert.AreEqual(
                replacement,
                text.ToHtml()
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void AssociatedTextReplacingGuidHasHtmlEscapableCharactersReplaced()
        {
            Guid toReplace = Guid.NewGuid();
            string replacement = "This <is> html?";
            string expectedOutput = "This &lt;is&gt; html?";
            Dictionary<Guid, string> replacements = new Dictionary<Guid, string>
            {
                {toReplace, replacement}
            };
            MarkdownText text = MarkdownText.EscapingReplacedHtml(
                toReplace.ToString(),
                replacements
            );
            Assert.AreEqual(
                expectedOutput,
                text.ToHtml()
            );
        }
    }
}