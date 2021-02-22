
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MarkdownToHtml
{
    public class Assertions
    {
        private static int showMisMatchCharacterSurroundings = 50;
    
        public static void AssertStringsEqualShowingDifference(
            string expected,
            string actual
        ) {
            if (expected == actual) {
                return;
            }
            int differenceIndex = 0;
            while (
                differenceIndex < expected.Length 
                    && differenceIndex < actual.Length
                    && expected.Substring(differenceIndex, 1) == actual.Substring(differenceIndex, 1)
             ) {
                 differenceIndex++;
             }
             int expectedLowerBound = Math.Max(0, differenceIndex - (showMisMatchCharacterSurroundings / 2));
             int actualLowerBound = Math.Max(0, differenceIndex - (showMisMatchCharacterSurroundings / 2));
             int expectedUpperBound = Math.Min(expected.Length, differenceIndex + (showMisMatchCharacterSurroundings / 2));
             int actualUpperBound = Math.Min(actual.Length, differenceIndex + (showMisMatchCharacterSurroundings / 2));
             string differenceMessage = "\nAt index " + differenceIndex
                 + "\nExpected <...>" + expected.Substring(expectedLowerBound, expectedUpperBound - expectedLowerBound) 
                 + "<...>\nActual   <...>" + actual.Substring(actualLowerBound, actualUpperBound - actualLowerBound) + "</...>\n";
            Assert.AreEqual(
                expected,
                actual,
                differenceMessage
            );
        }
    }
}