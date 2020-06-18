
namespace MarkdownToHtml
{
    public class Attribute
    {
        public string Name
        {
            get; private set;
        }

        public string Value
        {
            get; private set;
        }

        public Attribute(
            string name,
            string value
        ) {
            Name = name;
            Value = value;
        }
    }
}