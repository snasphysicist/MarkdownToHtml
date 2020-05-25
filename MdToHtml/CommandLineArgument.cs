
using System.Text.RegularExpressions;

namespace MdToHtml
{
    public class CommandLineArgument
    {
        private static Regex regexArgument = new Regex(
            @"^\s?[-|/]([A-Z|a-z])(\s+(\S*)\s*)?$"
        );

        public string Flag
        { get; private set; }

        public string Value
        { get; private set; }

        public bool Valid
        { get; private set; }

        public string Input
        { get; private set; }

        public CommandLineArgument(
            string argument
        ) {
            Input = argument;
            Match contentMatch = regexArgument.Match(Input);
            Valid = contentMatch.Success;
            if (Valid)
            {
                Flag = contentMatch.Groups[1].Value.ToLower();
                Value = contentMatch.Groups[3].Value;
            } else {
                Flag = "";
                Value = "";
            }
        }
    }
}