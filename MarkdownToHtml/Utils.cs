
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class Utils
    {
        public static T[] LinkedListToArray<T>(
            LinkedList<T> linkedList
        ) {
            T[] array = new T[linkedList.Count];
            linkedList.CopyTo(array, 0);
            return array;
        }
    }
}