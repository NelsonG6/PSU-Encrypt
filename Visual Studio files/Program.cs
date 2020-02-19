using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    partial class Program
    {        
        //text to pass through encryption
        static string InputText;

        //text to be encrypted converted to bits
        static string PreEncryptedBits;

        //decryption key, 64 bits, hex format
        static string InputKey;

        //Stores a list of 64 bit plaintext converted to bits, with padding in the last entry if necessary
        static List<string> PreEncryptionBitsList;

        //Stores a list of encrypted bits
        //Used for creating a string of hex characters in the case of encryption,
        //Or for translating to ascii for decryption.
        static List<string> PostEncryptionBitsList;

        //16 rounds of encryption
        static readonly int RoundCount = 16;

        //Counter for which round we're at
        static int CurrentRound;

        //A list of rounds, and the values from each round. Helps debug.
        static List<round> Rounds;     

        static void Main(string[] args)
        {
            bool KeepLooping = true;
            while (KeepLooping)
            {
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
                    InputText = System.IO.File.ReadAllText(@"plaintext.txt");

                    if(InputText.ElementAt(InputText.Length-1) == ' ')
                    {
                        while(InputText.ElementAt(InputText.Length - 1) == ' ')
                        {
                            InputText = InputText.Remove(InputText.Length - 1);
                        }

                        Console.WriteLine("\tTrailing spaces removed from the end of the input string.");
                    }

                    Console.WriteLine("\tReading key from file.");
                    InputKey = System.IO.File.ReadAllText(@"key.txt");

                    Console.WriteLine("\tPlaintext:\n\t\t" + InputText);
                    Console.WriteLine("\tKey:\n\t\t" + InputKey + "\n");

                    //Make a list of subkeys
                    InitializeSubkeys();

                    //Initialize plaintext
                    InitializePlaintextEncryption();
                }
                else
                {
                    Console.WriteLine("Decryption selected.");

                    Console.WriteLine("\tReading ciphertext from file.");
                    InputText = System.IO.File.ReadAllText(@"ciphertext.txt");

                    Console.WriteLine("\tReading key from file.");
                    InputKey = System.IO.File.ReadAllText(@"key.txt");

                    Console.WriteLine("\tCiphertext:\n\t\t" + InputText);
                    Console.WriteLine("\tKey:\n\t\t" + InputKey);

                    //Make a list of subkeys
                    InitializeSubkeys();

                    //Reverse key order for decryption
                    Console.WriteLine("\tReversing order of subkeys.\n");
                    Array.Reverse(Subkeys);

                    InitializeCiphertext();                   
                }

                
                int Counter = 0;

                //Stores bits after each 64-bt block.
                PostEncryptionBitsList = new List<string>();

                //Loop through the list of 64 bit chunks and run the encryption algorithm on them.
                while (Counter < PreEncryptionBitsList.Count)
                {
                    Rounds = new List<round>();
                    CurrentRound = 0;

                    //Create the first round
                    var FirstRound = new round();

                    //Input the plaintext converted to bits
                    FirstRound.InputListOfBitStrings = Split(PreEncryptionBitsList[Counter], 4).ToArray();

                    //Whiten the plaintext
                    FirstRound.WhitenInput();

                    //Primary round permutation loop
                    //Loop through 16 rounds
                    while (CurrentRound < RoundCount)
                    {
                        if (Rounds.Count == 0)
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

                    //Store this encryption in an ongoing list for final parsing
                    PostEncryptionBitsList.Add(string.Join("", Rounds.Last().OutputListOfBitStrings));

                    Counter++;
                }

                if (input.Contains("1"))
                {
                    //encryption was selected
                    //string encryptedBinary = string.Join("", Rounds.Last().OutputListOfBitStrings);

                    //Loop through encryptedbistlist and get hex from each bitstring
                    List<string> encryptedHexList = new List<string>();
                    string tempHex;
                    string hexString;

                    for (int i = 0; i < PostEncryptionBitsList.Count; i++)
                    {
                        //Split the list of bits into sections of 4, for hex conversion
                        var splitBits = Split(PostEncryptionBitsList[i], 16).ToArray();
                        hexString = "";

                        for(int j = 0; j < splitBits.Length; j++)
                        {
                            tempHex = GetHexStringFromBitString(splitBits[j]);
                            hexString += tempHex;
                        }

                        encryptedHexList.Add(hexString);
                    }

                    string encryptedHex = String.Join("", encryptedHexList.ToArray());

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
                    string decryptedString = "";
                    string temp;

                    for (int i = 0; i < PostEncryptionBitsList.Count; i++)
                    {
                        temp = GetHexStringFromBitString(PostEncryptionBitsList[i]);
                        decryptedString += GetAsciiFromHex(temp);
                    }

                    //Remove empty spaces at the end, which were only put there for padding
                    while(decryptedString.ElementAt(decryptedString.Length-1) == ' ')
                    {
                        decryptedString = decryptedString.Remove(decryptedString.Length - 1);
                    }

                    Console.WriteLine("Decryption complete.");
                    Console.WriteLine("\tDecrypted string:\n\t\t" + decryptedString);
                    Console.WriteLine("\tWriting to plaintext.txt\n");
                    Console.WriteLine("Press enter to finish.");
                    System.IO.File.WriteAllText(@"plaintext.txt", decryptedString);
                    Console.ReadLine();
                }

                Console.WriteLine("Run again (1) or quit(2)?");
                input = Console.ReadLine();
                Console.WriteLine();

                if(input.Contains("1"))
                {
                    Console.WriteLine("Running again.");
                }
                else
                {
                    Console.WriteLine("Quitting.");
                    Console.WriteLine("Press enter to finish.");
                    KeepLooping = false;
                }
            }
        }
    }
}