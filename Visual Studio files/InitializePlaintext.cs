using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    partial class Program
    {
        static public string[] InitializePlaintextEncryption()
        {
            //Need to right-shift all plaintext values, as there is a 0 added to the end
            //Split the string into lengths of 8

            PlaintextBits = GetBits(Plaintext);
            var PlaintextBitsDivided = Split(PlaintextBits, 8).ToArray();

            //Right shift here
            for (int i = 0; i < PlaintextBitsDivided.Length; i++)
            {
                PlaintextBitsDivided[i] = RightShift(PlaintextBitsDivided[i]);
            }

            //Join again to a single string so we can get 16 bit word lengths

            var RightShifted = string.Join("", PlaintextBitsDivided);

            var PlaintextBitsDivided4 = Split(RightShifted, 4);

            return PlaintextBitsDivided4.ToArray();
        }

        static string[] InitializeCiphertext()
        {
            PlaintextBits = GetBitsFromHex(Plaintext);
            var CiphertextBitsDivided4 = Split(PlaintextBits, 4);

            return CiphertextBitsDivided4.ToArray();

        }
    }
}
