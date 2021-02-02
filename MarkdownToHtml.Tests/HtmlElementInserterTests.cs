
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
    }
}