using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    partial class Program
    {
        class round
        {
            int round_number = 0;

            public string StartingStringBinary;
            public string EndingStringBinary;
            public string StartingStringHex;
            public string EndingStringHex;

            //This will be the output from the previous round
            public string[] InputListOfBitStrings = new string[4];

            //This will be the input for the subsequent round
            public string[] OutputListOfBitStrings = new string[4];

            public string F0;
            public string F1;

            public string F0hex;
            public string F1hex;

            public string T0;
            public string T1;

            public string T0hex;
            public string T1hex;

            public string[] Gvals = new string[12];
            public string[] GvalsHex = new string[12];

            public string[] Keys = new string[12];
            public string[] KeysHex = new string[12];
            public round()
            {

            }
            public round(round CopyFrom, int RoundNumber)
            {
                //Copy this round's input bits as the output of last round
                StartingStringBinary = string.Copy(CopyFrom.EndingStringBinary);
                StartingStringHex = string.Copy(CopyFrom.EndingStringHex);

                InputListOfBitStrings = Split(StartingStringBinary, 4).ToArray();

                round_number = RoundNumber;
            }

            public void SwapOutput()
            {
                string[] temp = new string[4];
                Array.Copy(OutputListOfBitStrings, 0, temp, 0, OutputListOfBitStrings.Length);

                OutputListOfBitStrings[0] = temp[2];
                OutputListOfBitStrings[1] = temp[3];
                OutputListOfBitStrings[2] = temp[0];
                OutputListOfBitStrings[3] = temp[1];

                //Populate hex values

                EndingStringBinary = string.Join("", OutputListOfBitStrings);
                EndingStringHex = GetHexStringFromBitString(EndingStringBinary);


            }

            public void WhitenOutput()
            {
                var SplitKey = Split(CurrentKeyBits, 4);
                OutputListOfBitStrings[0] = Xor(SplitKey[0], OutputListOfBitStrings[0]);
                OutputListOfBitStrings[1] = Xor(SplitKey[1], OutputListOfBitStrings[1]);
                OutputListOfBitStrings[2] = Xor(SplitKey[2], OutputListOfBitStrings[2]);
                OutputListOfBitStrings[3] = Xor(SplitKey[3], OutputListOfBitStrings[3]);
            }

            //Called once at the start
            public void WhitenInput()
            {
                var SplitKey = Split(CurrentKeyBits, 4);
                InputListOfBitStrings[0] = Xor(SplitKey[0], InputListOfBitStrings[0]);
                InputListOfBitStrings[1] = Xor(SplitKey[1], InputListOfBitStrings[1]);
                InputListOfBitStrings[2] = Xor(SplitKey[2], InputListOfBitStrings[2]);
                InputListOfBitStrings[3] = Xor(SplitKey[3], InputListOfBitStrings[3]);

                //Populate string values while we're here
                StartingStringBinary = string.Join("", InputListOfBitStrings);
                StartingStringHex = GetHexStringFromBitString(StartingStringBinary);
            }

            public void XorF0F1()
            {
                OutputListOfBitStrings[2] = Xor(InputListOfBitStrings[2], F0);
                OutputListOfBitStrings[3] = Xor(InputListOfBitStrings[3], F1);
                OutputListOfBitStrings[0] = InputListOfBitStrings[0];
                OutputListOfBitStrings[1] = InputListOfBitStrings[1];
            }
        }

    }
}
