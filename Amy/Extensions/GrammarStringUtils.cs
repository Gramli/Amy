using System;
using System.Text;

namespace Amy.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveCharExceptTerminals(this string rule, char character)
        {
            var dontSkip = false;
            var result1 = new StringBuilder();
            for (int i = 0; i < rule.Length; i++)
            {
                if (rule[i].Equals(character) && !dontSkip)
                {
                    continue;
                }
                var quote = rule[i].Equals('"');
                if (quote && dontSkip)
                {
                    dontSkip = false;
                }
                else if (quote && !dontSkip)
                {
                    dontSkip = true;
                }

                result1.Append(rule[i]);
            }

            return result1.ToString();
        }

        public static string RemoveNewLines(this string rule)
        {
            return rule.Replace(Environment.NewLine, string.Empty);
        }

        public static string RemoveSpaces(this string rule)
        {
            return RemoveCharExceptTerminals(rule, ' ');
        }
    }
}
