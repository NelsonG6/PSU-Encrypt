using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    //Left to do:
    
    //Handle input block sizes greater than 64 bits
    //Loop through for each block size?

    //Set option to decrypt or encrypt only

    partial class Program
    {        
        //text to be encrypted
        static string Plaintext;

        //text to be encrypted converted to bits - needs right shift here
        static string PlaintextBits;

        //decryption key, 64 bits, hex format
        static string InputKey;

        //decryption key converted to binary
        //storing this for reference, won't modify this value
        //static string KeyBits;

        static readonly int RoundCount = 16;
        static int CurrentRound = 0;

        static List<round> Rounds;     

        static void Main(string[] args)
        {
            string[] PlaintextBitsDivided4;
            Console.WriteLine("Will you be encrypting (1) or decrypting (2)?");
            var input = Console.ReadLine();
            Console.WriteLine();
            if (input.Contains("1"))
            {
                //Encrypting
                //Grab plaintext from plaintext.txt
                //Grab key from key.txt
                Console.WriteLine("Encryption selected.");
                Console.WriteLine("\tReading plaintext from file.");
                Plaintext = System.IO.File.ReadAllText(@"plaintext.txt");

                Console.WriteLine("\tReading key from file.");
                InputKey = System.IO.File.ReadAllText(@"key.txt");

                Console.WriteLine("\tPlaintext:\n\t\t" + Plaintext);
                Console.WriteLine("\tKey:\n\t\t" + InputKey + "\n");

                InitializeSubkeys();

                //Initialize plaintext
                PlaintextBitsDivided4 = InitializePlaintextEncryption();
            }
            else
            {
                Console.WriteLine("Decryption selected.");

                Console.WriteLine("\tReading ciphertext from file.");
                Plaintext = System.IO.File.ReadAllText(@"ciphertext.txt");

                Console.WriteLine("\tReading key from file.");
                InputKey = System.IO.File.ReadAllText(@"key.txt");

                Console.WriteLine("\tCiphertext:\n\t\t" + Plaintext);
                Console.WriteLine("\tKey:\n\t\t" + InputKey);

                InitializeSubkeys();

                Console.WriteLine("\tReversing order of subkeys.\n");

                PlaintextBitsDivided4 = InitializeCiphertext();

                Array.Reverse(Subkeys);
            }

            //KeyBits = GetBitsFromHex(InputKey);
            


            Rounds = new List<round>();

            //Create the first round
            var FirstRound = new round();

            //Input the plaintext converted to bits
            FirstRound.InputListOfBitStrings = PlaintextBitsDivided4.ToArray();

            //Whiten the plaintext
            FirstRound.WhitenInput();

            //Primary round permutation loop
            //Encryption

            while(CurrentRound < RoundCount)
            {
                if(Rounds.Count == 0)
                    Rounds.Add(FirstRound);
                else
                    Rounds.Add(new round(Rounds.Last(), CurrentRound));

                //Reset the key index that loops through the 12 keys
                CurrentKey = 0;
                
                F(); //Run the F function, which will populate F0 and F1 values.

                Rounds.Last().XorF0F1();

                //Swap the sides now that our Xoring is complete
                Rounds.Last().SwapOutput();

                CurrentRound++;
            }

            //Rounds are done.
            //Re-swap the last round.
            Rounds.Last().SwapOutput();

            //Whiten against the current key.
            Rounds.Last().WhitenOutput();
            
            //Convert binary stored to hex
            

            if(input.Contains("1"))
            {
                //encryption was selected
                string encrypted = string.Join("", Rounds.Last().OutputListOfBitStrings);
                string encryptedHex = GetHexStringFromBitString(encrypted);

                Console.WriteLine("Encryption complete.");
                Console.WriteLine("\tEncrypted text:\n\t\t" + encryptedHex);

                Console.WriteLine("\tWriting to ciphertext.txt\n");
                Console.WriteLine("Press enter to finish.");
                System.IO.File.WriteAllText(@"ciphertext.txt", encryptedHex);
                Console.ReadLine();
            }
            else
            {
                //decryption
                string decrypted = string.Join("", Rounds.Last().OutputListOfBitStrings);
                string decryptedHex = GetHexStringFromBitString(decrypted);
                string decryptedString = GetCharacterStringFromHexString(decryptedHex);

                Console.WriteLine("Decryption complete.");
                Console.WriteLine("\tDecrypted string:\n\t\t" + decryptedString);
                Console.WriteLine("\tWriting to plaintext.txt\n");
                Console.WriteLine("Press enter to finish.");
                System.IO.File.WriteAllText(@"plaintext.txt", decryptedString);
                Console.ReadLine();
            }
        }
    }
}