
using System.Text.Regex;

namespace MdToHtml
{
    public class CommandLineArgument
    {
        Regex regexArgument = new Regex(
            @"^[-|/]([A-Z|a-z])\s+(\S*)\s*$"
        );

        public string Flag
        { get; private set; }

        public string Value
        { get; private set; }

        public boolean Valid
        { get; private set; }

        public CommandLineArgument(
            string argument
        ) {
            Match contentMatch = regexArgument.Match(argument);
            Valid = contentMatch.Success;
            if (Valid)
            {
                Flag = contentMatch.Groups[1].Value;
                Value = contentMatch.Groups[2].Value;
            } else {
                Flag = "";
                Value = "";
            }
        }
    }
}