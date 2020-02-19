using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    partial class Program
    {
        static public void InitializePlaintextEncryption()
        {
            //Get an array of all the characters in the input string represented as bytes.
            //Each character is one byte.
            byte[] bytes = Encoding.ASCII.GetBytes(InputText);

            //Build an array of 64-bit binary strings.
            //Earch array needs to be 64 bits, so if it doesn't have 8 bytes worth, we will pad with space.
            //When decrypting, we'll remove the space from the end of the resulting text.

            string building = "";
            string tempString;
            //Split the string into lengths of 8
            PreEncryptionBitsList = new List<string>();

            int i = 0;

            int counter = 0;
            //Only loop as long as we have bytes left
            while (counter < bytes.Length)
            {
                building = "";
                //Loop 8 bytes at a time, but stop if counter gets too high.
                for (i = 0; i < 8 & counter < bytes.Length; i++)
                {
                    tempString = Convert.ToString(bytes[counter], 2);
                    while(tempString.Length < 8)
                    {
                        //If our binary string representing an ascii value is not 8 bits, pad it with 0 at the front.
                        tempString = tempString.Insert(0, "0");
                    }
                    building += tempString;
                    counter++;
                }

                //If i is less than 7, then we did not have a number of bytes exactly divisible by 8.
                //This means we need to pad.
                if (i < 8)
                {
                    byte temp;

                    while (i < 8)
                    {
                        //Pad with space
                        temp = Encoding.ASCII.GetBytes(" ")[0];
                        tempString = Convert.ToString(temp, 2);
                        while (tempString.Length < 8)
                        {
                            //If our binary string representing an ascii value is not 8 bits, pad it with 0 at the front.
                            tempString = tempString.Insert(0, "0");
                        }
                        building += tempString;
                        i++;
                    }                    
                }

                PreEncryptionBitsList.Add(string.Copy(building));
            }
        }

        static void InitializeCiphertext()
        {
            //The result of encryption will always be a block that is a multiple of 64 bits
            PreEncryptedBits = GetBitsFromHex(InputText);

            //Get a number of how many 64 bit chunks there are
            int chunkSize = PreEncryptedBits.Length / 64;

            //Create a list of bits
            PreEncryptionBitsList = Split(PreEncryptedBits, chunkSize);
        }
    }
}
