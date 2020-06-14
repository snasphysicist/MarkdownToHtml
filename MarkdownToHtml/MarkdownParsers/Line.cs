
namespace MarkdownToHtml
{
    public class Line
    {
        private bool hasBeenParsed;

        public string Text
        { get; set; }

        public Line(
            string text
        ) {
            Text = text;
            hasBeenParsed = false;
        }

        public bool ContainsOnlyWhitespace()
        {
            return Utils.ContainsOnlyWhitespace(
                Text
            );
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