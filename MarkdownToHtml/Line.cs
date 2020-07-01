
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
            if (text == "")
            {
                hasBeenParsed = true;
            } else {
                hasBeenParsed = false;
            }
            
        }

        public bool ContainsOnlyWhitespace()
        {
            return Text.Replace(
                " ",
                ""
            ).Length == 0;
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