
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlSubstitutionTests
    {

        private HtmlElement[] HtmlStringToElements(
            string html
        ) {
            HtmlToken[] tokens = new HtmlTokeniser(html).tokenise();
            HtmlSnippet[] snippets = HtmlTagDetector.TagsFromTokens(tokens);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
            return elements;
        }

        private Guid[] Keys(
            Dictionary<Guid, string> replacements
        ) {
            Guid[] guids = new Guid[replacements.Count];
            replacements.Keys.CopyTo(guids, 0);
            return guids;
        }

        private string[] Values(
            Dictionary<Guid, string> replacements
        ) {
            string[] strings = new string[replacements.Count];
            replacements.Values.CopyTo(strings, 0);
            return strings;
        }

        private Guid FindKeyForValueOrDefault(
            Dictionary <Guid, string> replacements,
            string search
        ) {
            foreach (KeyValuePair<Guid, string> entry in replacements)
            {
                if (entry.Value == search)
                {
                    return entry.Key;
                }
            }
            throw new AssertFailedException("The provided exact string \"" + search + "\" was not in the list of replaced values");
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("Text")]
        [DataRow(" ")]
        [DataRow("\n")]
        [DataRow("<")]
        [DataRow(">")]
        [DataRow("/")]
        [DataRow("=")]
        [DataRow("\"")]
        [DataRow("<p>")]
        [DataRow("</p>")]
        public void TextWhichDoesNotFormAValidHtmlElementIsNotReplacedByAnything(
            string html
        ) {
            HtmlElement[] elements = HtmlStringToElements(html);
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            Assert.AreEqual(
                html,
                substituter.Processed
            );
            Assert.AreEqual(
                0,
                substituter.GetReplacements().Count
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void SingleValidBlockLevelElementWithoutNestedElementsIsReplacedByLineBreaksAndUUID()
        {
            string html = "\n\n<p>Test</p>\n\n";
            HtmlElement[] elements = HtmlStringToElements(html);
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            Assert.AreEqual(
                1,
                substituter.GetReplacements().Count
            );
            Guid uuid = Keys(substituter.GetReplacements())[0];
            string substituted = Values(substituter.GetReplacements())[0];
            Assert.AreEqual(
                html.Replace("\n", ""),
                substituted
            );
            Assert.AreEqual(
                "\n\n" + uuid.ToString() + "\n\n",
                substituter.Processed
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("\n\n<p>Outside<p>then in</p>and back</p>\n\n")]
        [DataRow("\n\n<div>We<p>need<article>to</article>go</p>deeper</div>\n\n")]
        [DataRow("\n\n<p>I can't <b>emphasise</b> this enough</p>\n\n")]
        [DataRow("\n\n<div>Extremely <i>very <b>really</b> <span>insanely</span></i> important!</div>\n\n")]
        public void SingleValidBlockLevelElementWithNestedElementsIsReplacedByLineBreaksAndUUID(
            string html
        ) {
            HtmlElement[] elements = HtmlStringToElements(html);
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            Assert.AreEqual(
                1,
                substituter.GetReplacements().Count
            );
            Guid uuid = Keys(substituter.GetReplacements())[0];
            string substituted = Values(substituter.GetReplacements())[0];
            Assert.AreEqual(
                html.Replace("\n", ""),
                substituted
            );
            Assert.AreEqual(
                "\n\n" + uuid.ToString() + "\n\n",
                substituter.Processed
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void SingleValidInlineElementWithoutNestedElementsOnlyTagsAreReplacedByUUIDNotInnerText()
        {
            string html = "<span> Test </span>";
            HtmlElement[] elements = HtmlStringToElements(html);
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            Assert.AreEqual(
                2,
                substituter.GetReplacements().Count
            );
            Guid[] uuids = Keys(substituter.GetReplacements());
            string[] substituted = Values(substituter.GetReplacements());
            Guid openerUuid = new Guid();
            string openerSubstituted = "";
            Guid closerUuid = new Guid();
            string closerSubstituted = "";
            for (int i = 0; i < uuids.Length; i++)
            {
                if (substituted[i].Contains("/"))
                {
                    closerUuid = uuids[i];
                    closerSubstituted = substituted[i];
                } else {
                    openerUuid = uuids[i];
                    openerSubstituted = substituted[i];
                }
            }
            Assert.AreEqual(
                html.Split(" ")[0],
                openerSubstituted
            );
            Assert.AreEqual(
                substituter.Processed.Split(" ")[0],
                openerUuid.ToString()
            );
            Assert.AreEqual(
                html.Split(" ")[2],
                closerSubstituted
            );
            Assert.AreEqual(
                substituter.Processed.Split(" ")[2],
                closerUuid.ToString()
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void SingleValidInlineElementWithAttributesWithoutNestedElementsOnlyTagsAreReplacedByUUIDNotInnerText()
        {
            string opener = "<span a=\"b\">";
            string innerText = " Test ";
            string closer = "</span>";
            string html = opener + innerText + closer;
            HtmlElement[] elements = HtmlStringToElements(html);
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            Assert.AreEqual(
                2,
                substituter.GetReplacements().Count
            );
            Guid[] uuids = Keys(substituter.GetReplacements());
            string[] substituted = Values(substituter.GetReplacements());
            Guid openerUuid = new Guid();
            string openerSubstituted = "";
            Guid closerUuid = new Guid();
            string closerSubstituted = "";
            for (int i = 0; i < uuids.Length; i++)
            {
                if (substituted[i].Contains("/"))
                {
                    closerUuid = uuids[i];
                    closerSubstituted = substituted[i];
                } else {
                    openerUuid = uuids[i];
                    openerSubstituted = substituted[i];
                }
            }
            Assert.AreEqual(
                opener,
                openerSubstituted
            );
            Assert.AreEqual(
                substituter.Processed.Split(" ")[0],
                openerUuid.ToString()
            );
            Assert.AreEqual(
                closer,
                closerSubstituted
            );
            Assert.AreEqual(
                substituter.Processed.Split(" ")[2],
                closerUuid.ToString()
            );
            string processedInnerText = substituter.Processed;
            for (int i = 0; i < uuids.Length; i++)
            {
                processedInnerText = processedInnerText.Replace(uuids[i].ToString(), "");
            }
            Assert.AreEqual(
                innerText,
                processedInnerText
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void SingleValidInlineElementWithNestedElementsOuterAndNestedTagsNoInnerTextReplacedByUUID()
        {
            string outerOpener = "<span>";
            string innerOpener = "<i>";
            string innerCloser = "</i>";
            string outerCloser = "</span>";
            string innerText1 = " Use ";
            string innerText2 = " the ";
            string innerText3 = " force ";
            string html = outerOpener + innerText1 + innerOpener + innerText2 + innerCloser + innerText3 + outerCloser;
            HtmlElement[] elements = HtmlStringToElements(html);
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            Assert.AreEqual(
                4,
                substituter.GetReplacements().Count
            );
            Guid outerOpenerUuid = FindKeyForValueOrDefault(
                substituter.GetReplacements(),
                outerOpener
            );
            Assert.AreEqual(
                substituter.Processed.Split(" ")[0],
                outerOpenerUuid.ToString()
            );
            Guid innerOpenerUuid = FindKeyForValueOrDefault(
                substituter.GetReplacements(),
                innerOpener
            );
            Assert.AreEqual(
                substituter.Processed.Split(" ")[2],
                innerOpenerUuid.ToString()
            );
            Guid innerCloserUuid = FindKeyForValueOrDefault(
                substituter.GetReplacements(),
                innerCloser
            );
            Assert.AreEqual(
                substituter.Processed.Split(" ")[4],
                innerCloserUuid.ToString()
            );
            Guid outerCloserUuid = FindKeyForValueOrDefault(
                substituter.GetReplacements(),
                outerCloser
            );
            Assert.AreEqual(
                substituter.Processed.Split(" ")[6],
                outerCloserUuid.ToString()
            );
            Assert.AreEqual(
                innerText1.Replace(" ", ""),
                substituter.Processed.Split(" ")[1]
            );
            Assert.AreEqual(
                innerText2.Replace(" ", ""),
                substituter.Processed.Split(" ")[3]
            );
            Assert.AreEqual(
                innerText3.Replace(" ", ""),
                substituter.Processed.Split(" ")[5]
            );
        }
    }
}