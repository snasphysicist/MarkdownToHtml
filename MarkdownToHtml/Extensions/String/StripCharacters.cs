
namespace MarkdownToHtml
{
    public static class StripCharacters
    {
        public static string StripLeadingCharacters(
            this string stripFrom,
            char toStrip
        ) {
            int removed = 0;
            while (
                (removed < stripFrom.Length)
                && (stripFrom[removed] == toStrip)
            ) {
                removed++;
            }
            return stripFrom.Substring(removed);
        }

        public static string StripLeadingCharacterUpTo(
            this string stripFrom,
            char toStrip,
            int maximumNumberToRemove
        ) {
            int removed = 0;
            while (
                (removed < stripFrom.Length)
                && (removed < maximumNumberToRemove)
                && (stripFrom[removed] == toStrip)
            ) {
                removed++;
            }
            return stripFrom.Substring(removed);
        }

        public static string StripTrailingCharacters(
            this string stripFrom,
            char toStrip
        ) {
            int keepUpTo = stripFrom.Length - 1;
            while (
                (keepUpTo >= 0)
                && (stripFrom[keepUpTo] == toStrip)
            ) {
                keepUpTo--;
            }
            return stripFrom.Substring(
                0,
                keepUpTo + 1
            );
        }
    }
}