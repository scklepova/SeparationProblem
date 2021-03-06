﻿using System;
using System.Collections.Generic;
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

        public static int Count1(this string s)
        {
            var c = 0;
            foreach (var sym in s)
            {
                if (sym == '1')
                    c++;
            }
            return c;
        }

        public static string DraftToString(this string draft, string synonym0, string synonym1)
        {
            var w = "";
            foreach (var c in draft)
                if (c == '0')
                    w += synonym0;
                else w += synonym1;
            return w;
        }

        public static string GetInverse(this string word)
        {
            var s = "";
            foreach (var c in word)
            {
                if (c == '0')
                    s += '1';
                else s += '0';
            }
            return s;
        }

        public static int FactorCount(this string word, string factor)
        {
            var counter = 0;
            for(var i = 0; i < word.Length - factor.Length + 1; i++)
                if (word.Substring(i, factor.Length) == factor)
                    counter++;
            return counter;
        }

        public static long Get5Hash(this string word, IEnumerable<Automata> automatas)
        {
            var hash = 0;
            foreach (var automata in automatas)
                hash = hash*5 + automata.LastState(word);
            return hash;
        }
    }
}