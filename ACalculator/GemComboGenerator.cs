using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACalculator
{
    public class GemComboGenerator
    {
        public static List<string> GetCombinations(ref string[] options, int num)
        {
            if (options == null || options.Length <= 0)
            {
                return null;
            }

            if (num > options.Length)
            {
                return CombinationHelper(ref options, 0, options.Length);
            }

            return CombinationHelper(ref options, 0, num);
        }

        private static List<string> CombinationHelper(ref string[] options, int index, int n)
        {
            if (n <= 0 || index >= options.Length)
            {
                return null;
            }

            List<string> res = new List<string>();

            // Only use the index-th element in options for n times
            res.Add(String.Concat(Enumerable.Repeat(options[index] + ",", n - 1)) + options[index]);

            // Use index-th element in options for n-1 -> 1 times
            for (int i = n; i >= 1; i--)
            {
                string prefix = String.Concat(Enumerable.Repeat(options[index] + ",", i - 1)) + options[index];
                List<string> rest = CombinationHelper(ref options, index + 1, n - i);
                if (rest != null)
                {
                    foreach (string tail in rest)
                    {
                        res.Add(prefix + "," + tail);
                    }
                }
            }

            // Use the index-th element in options for 0 times
            List<string> exclusive = CombinationHelper(ref options, index + 1, n);
            if (exclusive != null)
            {
                res.AddRange(exclusive);
            }
            return res;
        }
    }
}
