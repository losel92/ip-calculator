using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectGF2
{
    class Program
    {
        static void Main(string[] args)
        {
            // 255.255.255.255 --> 11111111.11111111.11111111.11111111
            int[] intSec = new int[4];
            string[] inputExploded;
            string ipAddr = "";
            string ipAddrBin = "";
            string subMaskStr = "";
            string subMaskBin = "";
            char ipClass;

            //Introduction
            Console.WriteLine("Welcome to the Ip Calculator 2000");
            Console.WriteLine("With this tool you can insert any ip address and get back the network address and the number of usable hosts");
            Console.WriteLine("Please enter the Ip address either as dotted decimal or prefix");
            Console.WriteLine("Use these two templates as a reference:");
            Console.WriteLine("192.128.1.0/255.255.255.0  ||  192.128.1.0/24\n\n");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Start();

            void Start() {
                Console.Clear();
                ResetVariables();

                Console.Write("Insert IP Address: ");
                //Gets the Ip address to be manipulated
                string userInput = Console.ReadLine();

                MakeCalculations();

                Console.Clear();
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 3);
                Console.WriteLine("Ip Address (Decimal): " + ipAddr);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 3);
                Console.WriteLine("Subnet Mask (Decimal):" + subMaskStr);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 3);
                Console.WriteLine("Ip Address (Binary): " + ipAddrBin);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 3);
                Console.WriteLine("Subnet Mask (Binary)" + subMaskBin);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 3);
                Console.WriteLine("Class: " + ipClass);


                Console.ReadKey();
                Start();


                void MakeCalculations()
                {
                    //This means that the user placed the slash in a wrong place or has an invalid ip address
                    if (userInput.IndexOf("/") < 7 || userInput.IndexOf("/") > 15)
                    {
                        Start();
                    }
                    else
                    {
                        inputExploded = userInput.Split('/');

                        //this means that the user used more than one slash
                        if (inputExploded.Length > 2)
                        {
                            Start();
                        }
                        else
                        {
                            ipAddr = inputExploded[0];
                            string subMaskRaw = inputExploded[1];

                            //Gets each individual Ip field
                            string[] ipExploded = ipAddr.Split('.');
                            int[] ipExplodedInt = new int[4];
                            string[] ipBin = new string[4];

                            string[] subnetExploded = new string[4];
                            int[] subnetInt = new int[4];
                            string[] subnetBin = new string[4];

                            //This means the user is entering a prefix for the subnet
                            if (subMaskRaw.Length < 3 && subMaskRaw.Length > 0 && Convert.ToInt32(subMaskRaw) > 1 && Convert.ToInt32(subMaskRaw) < 31)
                            {
                                //Breaks the subnet mask into three sections
                                //Also creates a user-friendly string with points separating each section of the subnet mask
                                for (int i = Convert.ToInt32(subMaskRaw), j = 0; j < 4; j++)
                                {
                                    int subSecBits = 0;
                                    //Defines how many bits there are in a given section
                                    if (i > 8)
                                    {
                                        subSecBits = 8;
                                        i -= 8;
                                    }
                                    else
                                    {
                                        subSecBits = i;
                                        i = 0;
                                    }

                                    //Translates from prefix to decimal and assigns it to the section
                                    int subSecInt = Convert.ToInt32(256 - Math.Pow(2, 8 - subSecBits));
                                    


                                    //Prefix to binary and int
                                    if (subSecBits == 0)
                                    {
                                        //If there are 0 bits then both the binary and the integer value should also be 0
                                        subnetBin[j] = "00000000";
                                        subnetInt[j] = 0;
                                    }
                                    //If there are more than 0 bits in a given section
                                    else
                                    {
                                        //binary
                                        for (int k = 0; k < subSecBits; k++)
                                        {
                                            subnetBin[j] += '1';
                                        }
                                        //int
                                        subnetInt[j] = subSecInt;
                                    }
                                    //Adds zeros to the end of the section, so every section consistently has 8 bits
                                    subnetBin[j] = subnetBin[j].PadRight(8, '0');

                                    //formats the SubMask in a nice way with points for showing the user
                                    subMaskStr += Convert.ToString(subSecInt) + '.';
                                    subMaskBin += subnetBin[j] + '.';
                                }
                                //Removes the last point on the string
                                subMaskStr = subMaskStr.TrimEnd('.');
                                subMaskBin = subMaskBin.TrimEnd('.');
                            }
                            //this means the subnet is a dotted decimal
                            else if (subMaskRaw.Length > 2 && subMaskRaw.Length <= 15)
                            {
                                //Assigns the subnet array to each individual
                                //value of the exploded string and convert them to integers
                                subnetExploded = subMaskRaw.Split('.');
                                subnetInt = testForErrors(subnetExploded);

                                //Gets the binary value of the subnet mask
                                subnetBin = intArrToBinArr(subnetInt);

                                //Beautifies the subMask binary to show to the user
                                foreach (string subMaskSec in subnetBin)
                                {
                                    subMaskBin += subMaskSec + '.';
                                }
                                subMaskBin = subMaskBin.TrimEnd('.');

                                //The one that's gonna be shown to the user
                                subMaskStr = subMaskRaw;
                            }
                            else
                            {
                                Start();
                            }

                            //Gets an array of the ip sections in decimal and one in binary
                            ipExplodedInt = testForErrors(ipExploded);
                            ipBin = intArrToBinArr(ipExplodedInt);
                            Console.ReadKey();

                            //Beautifies the binary ip address that's going to be shown to the user
                            foreach (string ipSec in ipBin)
                            {
                                ipAddrBin += ipSec + '.';
                            }
                            ipAddrBin = ipAddrBin.TrimEnd('.');

                            //Gets the class of the ip address
                            if (ipExplodedInt[0] >= 1 && ipExplodedInt[0] <= 127) { ipClass = 'A'; }
                            else if (ipExplodedInt[0] >= 128 && ipExplodedInt[0] <= 191) { ipClass = 'B'; }
                            else if (ipExplodedInt[0] >= 192 && ipExplodedInt[0] <= 223) { ipClass = 'C'; }
                            else if (ipExplodedInt[0] >= 224 && ipExplodedInt[0] <= 239) { ipClass = 'D'; }
                            else if (ipExplodedInt[0] >= 240 && ipExplodedInt[0] <= 255) { ipClass = 'E'; }
                            else { Start(); }

                            //Tests an Ip addr / sub mask for inconsistencies and returns an array with the values of each section
                            int[] testForErrors(string[] inputArr)
                            {
                                int[] inputExplodedInt = new int[4];

                                //This means the user hasn't punctuated the Ip address correctly
                                if (inputArr.Length > 4 || inputArr.Length < 4)
                                {
                                    Start();
                                }
                                else
                                {
                                    //If any ip section is not an integer, it will return an error
                                    //It will also test for a number that is in the range 0-255
                                    int j = 0;
                                    foreach (string sec in inputArr)
                                    {
                                        int i;
                                        if (!int.TryParse(sec, out i) || i > 255 || i < 0)
                                        {
                                            Start();
                                        }
                                        else
                                        {
                                            inputExplodedInt[j] = i;
                                        }
                                        j++;
                                    }
                                }

                                return inputExplodedInt;
                            }
                            //Converts an array of integers into an array of binary strings
                            string[] intArrToBinArr(int[] inputArr)
                            {
                                string[] outputBin = new string[4];
                                int i = 0;
                                foreach (int ipSec in inputArr)
                                {
                                    outputBin[i] = DecToBinary(inputArr[i]);
                                    outputBin[i] = outputBin[i].PadLeft(8, '0');
                                    i++;
                                }
                                return outputBin;
                            }
                        }
                    }
                }
            }

            //Converts from dotted decimal to dotted binary
            string DottedToBinary(string dotted)
            {
                //Splits the ip address into four separate strings
                string[] singleBinSec = dotted.Split('.');

                string final = "";

                for(int i = 0; i < singleBinSec.Length; i++)
                {
                    string binSec = DecToBinary(Convert.ToInt32(singleBinSec[i]));
                    intSec[i] = Convert.ToInt32(binSec);
                    binSec = binSec.PadLeft(8, '0');
                    binSec += '.';
                    final += binSec;
                }
                //Removes the last dot in the string
                final = final.Substring(0, final.Length-2);

                return final;
            }

            //Converts a decimal number into a binary string
            string DecToBinary(int dec)
            {
                string binStr = "";

                while (dec >= 1)
                {
                    binStr = Convert.ToString(dec % 2) + binStr;
                    dec /= 2;
                }

                return binStr;
            }

            string DecToDotted()
            {
                return "";
            }

            void ResetVariables()
            {
                ipAddr = "";
                ipAddrBin = "";
                subMaskStr = "";
                subMaskBin = "";
            }
        }
    }
}
