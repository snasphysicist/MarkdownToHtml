
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlElementInserterTests
    {
        [TestMethod]
        [Timeout(500)]
        public void NoReplacementsAreMadeIfInputReplacementDictionaryIsEmpty()
        {
            string toProcess = Guid.NewGuid().ToString();
            HtmlElementInserter inserter = new HtmlElementInserter(
                new Dictionary<Guid, string>(),
                toProcess
            );
            Assert.AreEqual(
                toProcess,
                inserter.Processed
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void NoReplacementsAreMadeNothingInTheInputStringMatchesAGuidInTheDictionary()
        {
            string toProcess = "Not A Guid, A Guid " + Guid.NewGuid().ToString();
            Dictionary<Guid, string> replacements = new Dictionary<Guid, string>{
                {Guid.NewGuid(), "<p>"}
            };
            HtmlElementInserter inserter = new HtmlElementInserter(
                replacements,
                toProcess
            );
            Assert.AreEqual(
                toProcess,
                inserter.Processed
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void InputContainingOnlyAMatchingGuidIsReplacedByReplacementForThatGuid()
        {
            Guid guid = Guid.NewGuid();
            string toProcess = guid.ToString();
            string replaced = "<p>";
            Dictionary<Guid, string> replacements = new Dictionary<Guid, string>{
                {guid, replaced},
                {Guid.NewGuid(), "Something else"}
            };
            HtmlElementInserter inserter = new HtmlElementInserter(
                replacements,
                toProcess
            );
            Assert.AreEqual(
                replaced,
                inserter.Processed
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void InputContainingAMatchingGuidAmongOtherTextIsReplacedByReplacementForThatGuid()
        {
            Guid guid = Guid.NewGuid();
            string prefix = "Test ";
            string suffix = " Test " + Guid.NewGuid().ToString() + " Another Guid...";
            string toProcess = prefix + guid.ToString() + suffix;
            string replaced = "<p>";
            Dictionary<Guid, string> replacements = new Dictionary<Guid, string>{
                {guid, replaced},
                {Guid.NewGuid(), "Something else"}
            };
            HtmlElementInserter inserter = new HtmlElementInserter(
                replacements,
                toProcess
            );
            Assert.AreEqual(
                prefix + replaced + suffix,
                inserter.Processed
            );
        }
    }
}