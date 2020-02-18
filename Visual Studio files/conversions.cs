using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    partial class Program
    {
        static public List<string> Split(string str, int listSize)
        {
            List<string> StringList = new List<string>();
            int i = 0;
            int chunksize = str.Length / listSize;
            int pos1;

            while (i < listSize)
            {
                pos1 = (i * chunksize);

                StringList.Add(str.Substring(pos1, chunksize));
                i++;
            }
            return StringList;
        }

        public static string LeftShift(string t)
        {
            return t.Substring(1, t.Length - 1) + t.Substring(0, 1);
        }

        public static string RightShift(string t)
        {
            return t.Substring(t.Length - 1, 1) + t.Substring(0, t.Length - 1);
        }

        public static string GetBits(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in Encoding.Unicode.GetBytes(input))
            {
                sb.Append(Convert.ToString(b, 2));
            }
            return sb.ToString();
        }

        public static string GetBitsFromHex(string hexstring)
        {
            string binarystring = String.Join(String.Empty,
                hexstring.Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
            );

            return binarystring;
        }

        public static string Xor(string s1, string s2)
        {
            int longest = Math.Max(s1.Length, s2.Length);

            string first = s1.PadLeft(longest, '0');
            string second = s2.PadLeft(longest, '0');

            return string.Join("", Enumerable.Zip(first, second, (x, y) => x != y ? '1' : '0'));
        }

        public static string BinaryStringToUnicodeCharacter(string binaryInput)
        {
            return Encoding.UTF8.GetString(
            Regex.Split(
                binaryInput
                , "(.{8})") // this is the consequence of running them all together.
            .Where(binary => !String.IsNullOrEmpty(binary)) // keeps the matches; drops empty parts 
            .Select(binary => Convert.ToByte(binary, 2))
            .ToArray());
        }

        public static string GetHexStringFromBitString(string BitString)
        {
            string strHex = Convert.ToUInt64(BitString, 2).ToString("X");
            return strHex;
        }

        public static string GetCharacterStringFromHexString(string hexString)
        {
            if (hexString == null || (hexString.Length & 1) == 1)
            {
                throw new ArgumentException();
            }
            var sb = new StringBuilder();
            for (var i = 0; i < hexString.Length; i += 2)
            {
                var hexChar = hexString.Substring(i, 2);
                sb.Append((char)Convert.ToByte(hexChar, 16));
            }
            return sb.ToString();
        }
    }
}
