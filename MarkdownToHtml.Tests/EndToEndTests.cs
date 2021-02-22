
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

// Markdown input based on https://www.markdownguide.org/basic-syntax
namespace MarkdownToHtml
{
    [TestClass]
    public class EndToEndTests
    {
        private static int showMisMatchCharacterSurroundings = 40;

        private static string Parse(
            string markdown
        ) {
            MarkdownParser parser = new MarkdownParser(markdown);
            return parser.ToHtml();
        }

        private static void AssertStringsEqualShowingDifference(
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

        [TestMethod]
        [Timeout(500)]
        public void PlainMarkdown() {
            string markdown =
                "# Heading level 1\n\n" +
                "## Heading level 2\n\n" +
                "### Heading level 3\n\n" + 
                "#### Heading level 4\n\n" +
                "##### Heading level 5\n\n" + 
                "###### Heading level 6\n\n" +
                "Heading level 1\n===============\n\n" + 
                "Heading level 2\n---------------\n\n" +
                "I really like using Markdown.\n\n" +
                "I think I'll use it to format all of my documents from now on.\n\n" +
                "This is the first line.  \nAnd this is the second line.\n\n" +
                "This is the first line.\nThis is still the first line.\n\n" +
                "I just love **bold text**.\n\n" +
                "I just love __bold text__.\n\n" +
                "Love**is**bold\n\n" +
                "Love__is__bold\n\n" +
                "Italicised text is the *cat's meow*.\n\n" +
                "Italicised text is the _cat's meow_.\n\n" +
                "A*cat*meow\n\n" + 
                "A_cat_meow\n\n" +
                "This text is ***really important***.\n\n" +
                "This text is ___really important___.\n\n" + 
                "This text is __*really important*__.\n\n" +
                "This text is **_really important_**.\n\n" +
                "This is really***very***important text.\n\n" +
                "This is really___very___important text.\n\n" +
                "> Dorothy followed her through many of the beautiful rooms in her castle.\n\n" +
                "break 1\n\n" +
                "> Dorothy followed her through many of the beautiful rooms in her castle.\n>\n" + 
                "> The Witch bade her clean the pots and kettles and sweep the floor and keep the fire fed with wood.\n\n" +
                "break 2\n\n" +
                "> Dorothy followed her through many of the beautiful rooms in her castle.\n>\n" +
                ">> The Witch bade her clean the pots and kettles and sweep the floor and keep the fire fed with wood.\n\n" +
                "break 3\n\n" +
                "> #### The quarterly results look great!\n>\n> - Revenue was off the chart.\n" + 
                "> - Profits were higher than ever.\n>\n> *Everything* is going according to **plan**.\n\n" +
                "break 4\n\n" +
                "1. First item\n2. Second item\n3. Third item\n4. Fourth item\n\n" +
                "break 5\n\n" +
                "1. First item\n1. Second item\n1. Third item\n1. Fourth item\n\n" +
                "break 6\n\n" +
                "1. First item\n8. Second item\n3. Third item\n5. Fourth item\n\n" +
                "break 7\n\n" +
                "1. First item\n2. Second item\n3. Third item\n    1. Indented item\n    2. Indented item\n4. Fourth item\n\n" +
                "break 8\n\n" +
                "- First item\n- Second item\n- Third item\n- Fourth item\n\n" +
                "break 9\n\n" +
                "* First item\n* Second item\n* Third item\n* Fourth item\n\n" +
                "break 10\n\n" +
                "+ First item\n+ Second item\n+ Third item\n+ Fourth item\n\n" +
                "break 11\n\n" +
                "- First item\n- Second item\n- Third item\n    - Indented item\n    - Indented item\n- Fourth item\n\n" +
                "break 12\n\n" +
                "+ First item\n* Second item\n- Third item\n+ Fourth item\n\n" +
                "break 13\n\n" +
                "*   This is the first list item.\n*   Here's the second list item.\n\n" + 
                "    I need to add another paragraph below the second list item.\n\n*   And here's the third list item.\n\n" +
                "break 14\n\n" +
                "*   This is the first list item.\n*   Here's the second list item.\n\n" +
                "    > A blockquote would look great below the second list item.\n\n*   And here's the third list item.\n\n" +
                "break 15\n\n" +
                "1.  Open the file.\n2.  Find the following code block on line 21:\n\n" +
                "        <html>\n          <head>\n            <title>Test</title>\n" +
                "          </head>\n        </html>\n\n" +
                "3.  Update the title to match the name of your website.\n\n" +
                "break 16\n\n" +
                "1. Open the file containing the Linux mascot.\n2. Marvel at its beauty.\n\n" +
                "    ![Tux, the Linux mascot](/assets/images/tux.png)\n\n3. Close the file.\n\n" +
                "break 17\n\n" +
                "At the command prompt, type `nano`.\n\n" +
                "``Use `code` in your Markdown file.``\n\n" +
                "    <html>\n    <head>\n    </head>\n    </html>\n\n" +
                "***\n\n---\n\n___________________\n\n" +
                "My favourite search engine is [Duck Duck Go](https://duckduckgo.com).\n\n" +
                "But my favourite search engine is [Bing](https://bing.com \"The worst search engine, period\").\n\n" +
                "I love supporting the **[EFF](https://eff.org)**.\n\n" +
                "This is the *[Markdown Guide](https://www.markdownguide.org)*.\n\n" +
                "See the section on [`code`](#code).\n\n" +
                "[An article about the Hobbit Hole][1]\n\n" +
                "[An article about Dockstavarvet, a Swedish shipyard] [2]\n\n" +
                "[An article about the USS _Banner_][3]\n\n" +
                "[An article about the Dyers Almshouses][4]\n\n" +
                "[1]: https://en.wikipedia.org/wiki/Hobbit#Lifestyle\n\n" +
                "[2]: https://en.wikipedia.org/wiki/Dockstavarvet \"A cold, cold shipyard\"\n\n" +
                "[3]: https://en.wikipedia.org/wiki/USS_Banner_(AKL-25) 'Look at it in Hong Kong, how cool is that!'\n\n" +
                "[4]: https://en.wikipedia.org/wiki/Dyers_Almshouses (Charity)\n\n" +
                "![Philadelphia's Magic Gardens. This place was so cool!](/assets/images/philly-magic-gardens.jpg " + 
                "\"Philadelphia's Magic Gardens\")\n\n" +
                "[![An old rock in the desert](/assets/images/shiprock.jpg \"Shiprock, New Mexico by Beau Rogers\")]" + 
                "(https://www.flickr.com/photos/beaurogers/31833779864/in/photolist-Qv3rFw-34mt9F-a9Cmfy-5Ha3Zi-9msKdv-" + 
                "o3hgjr-hWpUte-4WMsJ1-KUQ8N-deshUb-vssBD-6CQci6-8AFCiD-zsJWT-nNfsgB-dPDwZJ-bn9JGn-5HtSXY-6CUhAL-a4UTXB-" + 
                "ugPum-KUPSo-fBLNm-6CUmpy-4WMsc9-8a7D3T-83KJev-6CQ2bK-nNusHJ-a78rQH-nw3NvT-7aq2qf-8wwBso-3nNceh-ugSKP-" + 
                "4mh4kh-bbeeqH-a7biME-q3PtTf-brFpgb-cg38zw-bXMZc-nJPELD-f58Lmo-bXMYG-bz8AAi-bxNtNT-bXMYi-bXMY6-bXMYv)\n\n" +
                "\\* Without the backslash, this would be a bullet in an unordered list.\n";
            string html = Parse(markdown);

            string expected = "<h1>Heading level 1</h1>\n" +
                "<h2>Heading level 2</h2>\n" +
                "<h3>Heading level 3</h3>\n" +
                "<h4>Heading level 4</h4>\n" +
                "<h5>Heading level 5</h5>\n" +
                "<h6>Heading level 6</h6>\n" +
                "<h1>Heading level 1</h1>\n" + 
                "<h2>Heading level 2</h2>\n" + 
                "<p>I really like using Markdown.</p>\n" +
                "<p>I think I&apos;ll use it to format all of my documents from now on.</p>\n" +
                "<p>This is the first line.<br>And this is the second line.</p>\n" +
                "<p>This is the first line. This is still the first line.</p>\n" +
                "<p>I just love <strong>bold text</strong>.</p>\n" +
                "<p>I just love <strong>bold text</strong>.</p>\n" +
                "<p>Love<strong>is</strong>bold</p>\n" +
                "<p>Love<strong>is</strong>bold</p>\n" + 
                "<p>Italicised text is the <em>cat&apos;s meow</em>.</p>\n" +
                "<p>Italicised text is the <em>cat&apos;s meow</em>.</p>\n" +
                "<p>A<em>cat</em>meow</p>\n" +
                "<p>A<em>cat</em>meow</p>\n" +
                "<p>This text is <strong><em>really important</em></strong>.</p>\n" +
                "<p>This text is <strong><em>really important</em></strong>.</p>\n" +
                "<p>This text is <strong><em>really important</em></strong>.</p>\n" +
                "<p>This text is <strong><em>really important</em></strong>.</p>\n" +
                "<p>This is really<strong><em>very</em></strong>important text.</p>\n" +
                "<p>This is really<strong><em>very</em></strong>important text.</p>\n" + 
                "<blockquote><p>Dorothy followed her through many of the beautiful rooms in her castle.</p>\n</blockquote>\n" +
                "<p>break 1</p>\n" +
                "<blockquote><p>Dorothy followed her through many of the beautiful rooms in her castle.</p>\n" + 
                "<p>The Witch bade her clean the pots and kettles and sweep the floor and keep the fire fed with wood.</p>\n" + 
                "</blockquote>\n" +
                "<p>break 2</p>\n" +
                "<blockquote><p>Dorothy followed her through many of the beautiful rooms in her castle.</p>\n" + 
                "<blockquote><p>The Witch bade her clean the pots and kettles and sweep the floor and keep the fire fed with wood.</p>" + 
                "\n</blockquote>\n</blockquote>\n" +
                "<p>break 3</p>\n" +
                "<blockquote><h4>The quarterly results look great!</h4>\n" + 
                "<ul><li>Revenue was off the chart.</li>\n<li>Profits were higher than ever.</li>\n</ul>\n" +
                "<p><em>Everything</em> is going according to <strong>plan</strong>.</p>\n</blockquote>\n" +
                "<p>break 4</p>\n" +
                "<ol><li>First item</li>\n<li>Second item</li>\n<li>Third item</li>\n<li>Fourth item</li>\n</ol>\n" +
                "<p>break 5</p>\n" +
                "<ol><li>First item</li>\n<li>Second item</li>\n<li>Third item</li>\n<li>Fourth item</li>\n</ol>\n" +
                "<p>break 6</p>\n" +
                "<ol><li>First item</li>\n<li>Second item</li>\n<li>Third item</li>\n<li>Fourth item</li>\n</ol>\n" +
                "<p>break 7</p>\n" +
                "<ol><li>First item</li>\n<li>Second item</li>\n<li>Third item<ol>" + 
                "<li>Indented item</li>\n<li>Indented item</li>\n</ol>\n</li>\n<li>Fourth item</li>\n</ol>\n" +
                "<p>break 8</p>\n" + 
                "<ul><li>First item</li>\n<li>Second item</li>\n<li>Third item</li>\n<li>Fourth item</li>\n</ul>\n" +
                "<p>break 9</p>\n" +
                "<ul><li>First item</li>\n<li>Second item</li>\n<li>Third item</li>\n<li>Fourth item</li>\n</ul>\n" +
                "<p>break 10</p>\n" +
                "<ul><li>First item</li>\n<li>Second item</li>\n<li>Third item</li>\n<li>Fourth item</li>\n</ul>\n" +
                "<p>break 11</p>\n" +
                "<ul><li>First item</li>\n<li>Second item</li>\n<li>Third item" + 
                "<ul><li>Indented item</li>\n<li>Indented item</li>\n</ul>\n</li>\n<li>Fourth item</li>\n</ul>\n" +
                "<p>break 12</p>\n" +
                "<ul><li>First item</li>\n<li>Second item</li>\n<li>Third item</li>\n<li>Fourth item</li>\n</ul>\n" +
                "<p>break 13</p>\n" +
                "<ul><li>This is the first list item.</li>\n<li><p>Here&apos;s the second list item.</p>\n" + 
                "<p>I need to add another paragraph below the second list item.</p>\n</li>\n" + 
                "<li><p>And here&apos;s the third list item.</p>\n</li>\n</ul>\n" +
                "<p>break 14</p>\n" +
                "<ul><li>This is the first list item.</li>\n<li><p>Here&apos;s the second list item.</p>\n" + 
                "<blockquote><p>A blockquote would look great below the second list item.</p>\n</blockquote>\n" + 
                "</li>\n<li><p>And here&apos;s the third list item.</p>\n</li>\n</ul>\n" +
                "<p>break 15</p>\n" +
                "<ol><li>Open the file.</li>\n<li><p>Find the following code block on line 21:</p>\n" + 
                "<pre><code>&lt;html&gt;\n  &lt;head&gt;\n    &lt;title&gt;Test&lt;/title&gt;\n" + 
                "  &lt;/head&gt;\n&lt;/html&gt;\n</code></pre>\n</li>\n" + 
                "<li><p>Update the title to match the name of your website.</p>\n</li>\n</ol>\n" +
                "<p>break 16</p>\n" +
                "<ol><li>Open the file containing the Linux mascot.</li>\n<li><p>Marvel at its beauty.</p>\n" +
                "<p><img src=\"/assets/images/tux.png\" alt=\"Tux, the Linux mascot\" title=\"\"></img></p>\n</li>\n" +
                "<li><p>Close the file.</p>\n</li>\n</ol>\n" +
                "<p>break 17</p>\n" +
                "<p>At the command prompt, type <code>nano</code>.</p>\n" +
                "<p><code>Use `code` in your Markdown file.</code></p>\n" +
                "<pre><code>&lt;html&gt;\n&lt;head&gt;\n&lt;/head&gt;\n&lt;/html&gt;\n</code></pre>\n" +
                "<hr />\n<hr />\n<hr />\n" + 
                "<p>My favourite search engine is <a href=\"https://duckduckgo.com\">Duck Duck Go</a>.</p>\n" +
                "<p>But my favourite search engine is <a href=\"https://bing.com\" title=\"The worst search engine, period\">Bing</a>.</p>\n" +
                "<p>I love supporting the <strong><a href=\"https://eff.org\">EFF</a></strong>.</p>\n" +
                "<p>This is the <em><a href=\"https://www.markdownguide.org\">Markdown Guide</a></em>.</p>\n" +
                "<p>See the section on <a href=\"#code\"><code>code</code></a>.</p>\n" + 
                "<p><a href=\"https://en.wikipedia.org/wiki/Hobbit#Lifestyle\" title=\"\">An article about the Hobbit Hole</a></p>\n" + 
                "<p><a href=\"https://en.wikipedia.org/wiki/Dockstavarvet\" title=\"A cold, cold shipyard\">" + 
                "An article about Dockstavarvet, a Swedish shipyard</a></p>\n" +
                "<p><a href=\"https://en.wikipedia.org/wiki/USS_Banner_(AKL-25)\" title=\"Look at it in Hong Kong, how cool is that!\">" + 
                "An article about the USS <em>Banner</em></a></p>\n" +
                "<p><a href=\"https://en.wikipedia.org/wiki/Dyers_Almshouses\" title=\"Charity\">An article about the Dyers Almshouses</a></p>\n" +
                "<p><img src=\"/assets/images/philly-magic-gardens.jpg\" " + 
                "alt=\"Philadelphia's Magic Gardens. This place was so cool!\" title=\"Philadelphia's Magic Gardens\"></img></p>\n" +
                "<p><a href=\"https://www.flickr.com/photos/beaurogers/31833779864/in/photolist-Qv3rFw-34mt9F-a9Cmfy-5Ha3Zi-9msKdv-" + 
                "o3hgjr-hWpUte-4WMsJ1-KUQ8N-deshUb-vssBD-6CQci6-8AFCiD-zsJWT-nNfsgB-dPDwZJ-bn9JGn-5HtSXY-6CUhAL-a4UTXB-" +
                "ugPum-KUPSo-fBLNm-6CUmpy-4WMsc9-8a7D3T-83KJev-6CQ2bK-nNusHJ-a78rQH-nw3NvT-7aq2qf-8wwBso-3nNceh-ugSKP-" +
                "4mh4kh-bbeeqH-a7biME-q3PtTf-brFpgb-cg38zw-bXMZc-nJPELD-f58Lmo-bXMYG-bz8AAi-bxNtNT-bXMYi-bXMY6-bXMYv" +
                "\"><img src=\"/assets/images/shiprock.jpg\" alt=\"An old rock in the desert\" " + 
                "title=\"Shiprock, New Mexico by Beau Rogers\"></img></a></p>\n" +
                "<p>\\* Without the backslash, this would be a bullet in an unordered list.</p>\n";
            AssertStringsEqualShowingDifference(
                expected,
                html
            );
        }
    }
}
