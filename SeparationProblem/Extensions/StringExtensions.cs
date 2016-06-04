using System;
using System.Linq;

namespace SeparationProblem.Extensions
{
    public static class StringExtensions
    {
        public static string GetReverse(this string s)
        {
            return new string(s.Reverse().ToArray());
        }

        public static bool HasEqualCountOf0And1(this string line)
        {
            var count0 = line.Count(x => x == '0');
            var count1 = line.Count(x => x == '1');

            return count0 == count1;
        }

        public static long ToLong(this string str)
        {
            var num = 0;
            foreach (var digit in str)
            {
                num = num * 2 + (digit - '0');
            }
            return num;
        }

        public static Tuple<string, string> GetPair(this string line)
        {
            var arr = line.Split(' ');
            return new Tuple<string, string>(arr[0], arr[1]);
        }

        public static bool IsEqualStringFor(this string line, string comparingLine)
        {
            return line == comparingLine || line == comparingLine.ComplementString();
        }

        public static string ComplementString(this string line)
        {
            var complement = "";
            foreach (var symbol in line)
            {
                if (symbol == '0')
                    complement += '1';
                else complement += '0';
            }
            return complement;
        }

        public static string BinaryPlusOne(this string line)
        {
            if (line == "")
                return "0";
            return  line.Select(x => x - '0').ToArray().PlusOneIn2ss(line.Length - 1).GetString();
        }
    }
}