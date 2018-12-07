using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACalculator
{
    class ACalculator
    {
        // Base attr
        private const double Base_ATK = 3000;
        private const double Base_CRT = 0.1;
        private const double Base_CRD = 0.5;

        // Attr per gem
        private const double ATK_GEM = 0.68;
        private const double CRT_GEM = 0.55;
        private const double CRD_GEM = 0.75;

        // Combo attr
        private static double GemCombo_ATK = 0;
        private static double GemCombo_CRT = 0;
        private static double GemCombo_CRD = 0;

        // Vice attr
        private static double Vice_ATK = 0;
        private static double Vice_CRT = 0;
        private static double Vice_CRD = 0;

        // Combinations
        //private static readonly string[] combinations =
        //{
        //    "ATK,ATK,ATK",
        //    "ATK,ATK,CRT",
        //    "ATK,ATK,CRD",
        //    "CRT,CRT,ATK",
        //    "CRT,CRT,CRT",
        //    "CRT,CRT,CRD",
        //    "CRD,CRD,ATK",
        //    "CRD,CRD,CRT",
        //    "CRD,CRD,CRD",
        //    "ATK,CRT,CRD"
        //};
        private static readonly List<string> combinations = GetCombinations(new string[]{"ATK", "CRT", "CRD"});

        private static List<string> GetCombinations (string[] options)
        {
            return CombinationHelper(options, 0, 3);
        }

        private static List<string> CombinationHelper(string[] options, int index, int n)
        {
            if (n <= 0 || index >= options.Length)
            {
                return null;
            }

            List<string> res = new List<string>();
            res.Add(String.Concat(Enumerable.Repeat(options[index] + ",", n - 1)) + options[index]);

            // Using options[index] for n-1 -> 1 times, record results
            for (int i = n; i >= 1; i--)
            {
                string prefix = String.Concat(Enumerable.Repeat(options[index] + ",", i - 1)) + options[index];
                List<string> rest = CombinationHelper(options, index + 1, n - i);
                if (rest != null)
                {
                    foreach (string tail in rest)
                    {
                        res.Add(prefix + "," + tail);
                    }
                }
            }

            // Not using options[index]
            List<string> exclusive = CombinationHelper(options, index + 1, n);
            if (exclusive != null)
            {
                res.AddRange(exclusive);
            } 
            return res;
        }

        private static void SetBaseAttr(Gem g, Dictionary<string, double> vice_attr)
        {
            Reset();
            if (g == Gem.Agile)
            {
                GemCombo_CRD = 0.2;
            }
            else if (g == Gem.Attack)
            {
                GemCombo_ATK = 0.2;
            }
            else if (g == Gem.Destroy)
            {
                GemCombo_CRD = 0.4;
            }

            foreach (string Key in vice_attr.Keys)
            {
                if (Key == "ATK")
                {
                    Vice_ATK += vice_attr[Key];
                }
                else if (Key == "CRT")
                {
                    Vice_CRT += vice_attr[Key];
                }
                else if (Key == "CRD")
                {
                    Vice_CRD += vice_attr[Key];
                }
            }
        }

        private static void Reset()
        {
            // Combo attr
            GemCombo_ATK = 0;
            GemCombo_CRT = 0;
            GemCombo_CRD = 0;

            // Vice attr
            Vice_ATK = 0;
            Vice_CRT = 0;
            Vice_CRD = 0;
        }

        public static void Calc(Gem g, Dictionary<string, double> vice_attr)
        {
            SetBaseAttr(g, vice_attr);
            foreach (string group in combinations)
            {
                double tmp_ATK_rate = GemCombo_ATK + Vice_ATK;
                double tmp_CRT = Base_CRT + GemCombo_CRT + Vice_CRT;
                double tmp_CRD = Base_CRD + GemCombo_CRD + Vice_CRD;

                foreach (string attr in group.Split(','))
                {
                    if (attr == "ATK")
                    {
                        tmp_ATK_rate += ATK_GEM;
                    }
                    else if (attr == "CRT")
                    {
                        tmp_CRT += CRT_GEM;
                    }
                    else if (attr == "CRD")
                    {
                        tmp_CRD += CRD_GEM;
                    }
                }

                double tmp_ATK = Base_ATK * (1 + tmp_ATK_rate);
                if (tmp_CRT > 1)
                {
                    tmp_CRT = 1;
                }
                //double final_atk = CalcExpectation(tmp_ATK, tmp_CRT, tmp_CRD);
                double final_atk = CalcExpectationWithDestroySkill(tmp_ATK, tmp_CRT, tmp_CRD);
                //double final_atk = CalcExpectationWithHighCRTSkill(tmp_ATK, tmp_CRT, tmp_CRD);
                Console.WriteLine(group + " : " + final_atk.ToString() + " ATK: " + tmp_ATK_rate.ToString() + " CRT: " + tmp_CRT.ToString() + " CRD: " + tmp_CRD.ToString());
            }


        }

        private static double CalcExpectation(double ATK, double CRT, double CRD)
        {
            double res = ATK * (1 + CRD) * CRT + ATK * (1 - CRT);
            return res;
        }

        private static double CalcExpectationWithDestroySkill(double ATK, double CRT, double CRD)
        {
            double res = ATK * (1 + CRD) * CRT * 1.5 + ATK * (1 - CRT);
            return res;
        }

        private static double CalcExpectationWithHighCRTSkill(double ATK, double CRT, double CRD)
        {
            double CRT_new = CRT + 0.3;
            if (CRT_new > 1)
            {
                CRT_new = 1;
            }
            double res = ATK * (1 + CRD) * CRT_new + ATK * (1 - CRT_new);
            return res;
        }

        public static void Main(string[] args)
        {
            //foreach (string group in ACalculator.combinations)
            //{
            //    Console.WriteLine(group);
            //}
            Dictionary<string, double> vice_attr = new Dictionary<string, double>()
            {
                {"ATK", 0.03},
                {"CRT", 0.2},
                {"CRD", 0.07},
            };
            Console.WriteLine("Attack gem");
            ACalculator.Calc(Gem.Attack, vice_attr);
            Console.WriteLine();
            Console.WriteLine("Agile gem");
            ACalculator.Calc(Gem.Agile, vice_attr);
            Console.WriteLine();
            Console.WriteLine("Destroy gem");
            ACalculator.Calc(Gem.Destroy, vice_attr);
            Console.ReadLine();
        }
    }

    enum Gem
    {
        Agile, Attack, Destroy
    }
}
