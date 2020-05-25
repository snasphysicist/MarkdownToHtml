
using System;

namespace MdToHtml
{
    class Program
    {
        private const string HelpText =
            "Markdown To HTML Conversion Utility\n\n"
            + "Usage: MdToHtml -i input_file -o output_file\n\n"
            + "Flags:\n"
            + "  -i input_file  -> path to Markdown file to be converted\n"
            + "  -o output_file -> path to which converted HTML file will be saved\n"
            + "  -h             -> print help text";

        static void Main(
            string[] args
        )
        {
            ArgumentParser arguments = new ArgumentParser(
                String.Join(
                    " ",
                    args
                )
            );
            if (
                !arguments.AllArgumentsValid()
                || arguments.HasValidFlag("h")
                || arguments.NoValidArguments()
            )
            {
                PrintHelp();
                System.Environment.Exit(0);
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine(
                HelpText
            );
        }
    }
}
