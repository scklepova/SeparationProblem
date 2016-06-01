using System.Linq;

namespace SeparationProblem.Extensions
{
    public static class ArrayExtensions
    {
        public static string GetString(this int[] array)
        {
            return array.Aggregate("", (current, i) => current + ((char)(i + '0')));
        }

        public static int[] PlusOneIn2ss(this int[] array, int pos)
        {
            var newArray = (int[])array.Clone();
            while (true)
            {
                if (pos < 0)
                {
                    newArray = new int[array.Length + 1];
                    for (var i = 0; i < array.Length + 1; i++)
                        newArray[i] = 0;
                    return newArray;
                }
                if (array[pos] + 1 < 2)
                {
                    newArray[pos]++;
                    return newArray;
                }

                newArray[pos] = 0;
                pos = pos - 1;
            }
        }

        public static string ToStr(this int[] array)
        {
            var str = "";
            foreach (var i in array)
            {
                str += i;
            }
            return str;
        }
    }
}