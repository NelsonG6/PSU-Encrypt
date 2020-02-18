using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    partial class Program
    {
        static int CurrentKey = 0;

        //key that is modified as function calls to K() is made
        static string CurrentKeyBits;

        static string[] CurrentKeyBitsArray;

        static string[][] Subkeys;

        //For debugging
        static string[][] ShiftedKeys;

        //For debugging
        static string[][] subkeysHex;

        static void InitializeSubkeys()
        {
            CurrentKeyBits = GetBitsFromHex(InputKey);

            Subkeys = new string[16][];
            ShiftedKeys = new string[16][];
            subkeysHex = new string[16][];

            //initialize subkeys
            for (int i = 0; i < Subkeys.Length; i++)
            {
                subkeysHex[i] = new string[12];
                Subkeys[i] = new string[12];
                ShiftedKeys[i] = new string[12];

            }

            //Loop through each round, and then loop through the 12 iterations for each round.
            for (int i = 0; i < RoundCount; i++)
            {
                //We loop through the four offsets three times.
                for(int k = 0; k < 3; k++)
                {
                    //In each round, we call k() four times, each with an incrementing offset
                    for (int j = 0; j < 4; j++)
                    {
                        //Left-shift the key.
                        CurrentKeyBits = LeftShift(CurrentKeyBits);

                        //Split the key into 8 parts, per instructions
                        CurrentKeyBitsArray = Split(CurrentKeyBits, 8).ToArray();

                        //reverse the key's bits, so K = K(7) + K(6) and so on, instead of the other way around
                        //This will need to be done every time we shift the key, as shifting happens in a single string
                        Array.Reverse(CurrentKeyBitsArray, 0, CurrentKeyBitsArray.Length);

                        Subkeys[i][k*4 + j] = K((4 * i) + j);
                        ShiftedKeys[i][k * 4 + j] = string.Copy(CurrentKeyBits);
                        subkeysHex[i][k * 4 + j] = GetHexStringFromBitString(Subkeys[i][k * 4 + j]);
                    }
                }
            }
        }

        static string K(int x)
        {
            var keyIndex = x % 8;
            return string.Copy(CurrentKeyBitsArray[keyIndex]);
        }

        static string GetCurrentKey()
        {
            if (CurrentKey > 11)
                throw new IndexOutOfRangeException();
            string returnval = Subkeys[CurrentRound][CurrentKey];
            Rounds.Last().KeysHex[CurrentKey] = GetHexStringFromBitString(returnval);
            Rounds.Last().Keys[CurrentKey] = returnval;
            CurrentKey++;
            return returnval;
        }
    }
}
