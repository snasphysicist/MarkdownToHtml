
using System.Text.RegularExpressions;

namespace MdToHtml
{
    public class CommandLineArgument
    {
        Regex regexArgument = new Regex(
            @"^[-|/]([A-Z|a-z])(\s+(\S*)\s*)?$"
        );

        public string Flag
        { get; private set; }

        public string Value
        { get; private set; }

        public bool Valid
        { get; private set; }

        public CommandLineArgument(
            string argument
        ) {
            Match contentMatch = regexArgument.Match(argument);
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