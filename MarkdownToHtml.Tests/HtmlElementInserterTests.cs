
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

    }
}