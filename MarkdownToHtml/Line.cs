
namespace MarkdownToHtml
{
    public class Line
    {
        private static int SPACES_PER_INDENTATION_LEVEL = 4;

        private bool hasBeenParsed;

        public string Original
        { get; private set; }

        public string Text
        { get; set; }

        public Line(
            string text
        ) {
            Text = text;
            Original = text;
            if (text == "")
            {
                hasBeenParsed = true;
            } else {
                hasBeenParsed = false;
            }
            
        }

        public int IndentationLevel() {
            return (Text.Length - Text.StripLeadingCharacters(' ').Length) 
                / SPACES_PER_INDENTATION_LEVEL;
        }

        public bool ContainsOnlyWhitespace()
        {
            return Text.ContainsOnlyWhitespace();
        }

        public bool HasBeenParsed(){
            return hasBeenParsed;
        }

        public void WasParsed()
        {
            hasBeenParsed = true;
        }

        // Wrapping the required string methods
        public bool StartsWith(
            string startText
        ) {
            return Text.StartsWith(
                startText
            );
        }

        public bool EndsWith(
            string startText
        ) {
            return Text.EndsWith(
                startText
            );
        }
    }
}