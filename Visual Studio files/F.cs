using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    partial class Program
    {
        static void F()
        {
            G();
            //The latest round now has T0 and T1

            //Get F0 and F1
            round ThisRound = Rounds.Last();

            string T0 = ThisRound.T0;
            int T0dec = Convert.ToInt32(T0, 2);

            string T1 = ThisRound.T1;           
            int T1dec = Convert.ToInt32(T1, 2);

            //Compute F0.
            string cat0 = GetCurrentKey();
            cat0 += GetCurrentKey();
            int cat0dec = Convert.ToInt32(cat0, 2);

            int F0_temp = T0dec + (2 * T1dec);
            F0_temp += cat0dec;
            F0_temp = F0_temp % (int)Math.Pow(2, 16);
            ThisRound.F0 = Convert.ToString(F0_temp, 2);
            ThisRound.F0hex = GetHexStringFromBitString(ThisRound.F0);

            //Compute F1
            string cat1 = GetCurrentKey();
            cat1 += GetCurrentKey();
            int cat1dec = Convert.ToInt32(cat1, 2);

            int F1_temp = (T0dec * 2) + T1dec;
            F1_temp += cat1dec;
            F1_temp = F1_temp % (int)Math.Pow(2, 16);
            ThisRound.F1 = Convert.ToString(F1_temp, 2);
            ThisRound.F1hex = GetHexStringFromBitString(ThisRound.F1);
        }
    }
}
