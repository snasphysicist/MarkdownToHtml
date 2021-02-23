
namespace MarkdownToHtml
{
    public static class StringContainsOnlyWhitespace
    {
        public static bool ContainsOnlyWhitespace(
            this string toCheck
        ) {
            return toCheck.Replace(
                " ",
                ""
            ).Length == 0;
        }
    }
}