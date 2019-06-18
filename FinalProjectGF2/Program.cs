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
            //Sets the color of the console
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;

            //Initial global variables
            int[] intSec = new int[4];
            string[] inputExploded;
            string ipAddr = "";
            string ipAddrBin = "";
            string subMaskStr = "";
            string subMaskBin = "";
            string networkAddr = "";
            string networkAddrBin = "";
            string errorMsg = "";
            int hosts = 0;
            int subnetsNo = 0;
            char ipClass;
            bool subnetExists = true;

            //Introduction
            Console.Clear();
            Console.WriteLine("Welcome to the Ip Calculator 2000");
            Console.WriteLine("With this tool you can insert any ip address and get back the network address and the number of usable hosts");
            Console.WriteLine("Please enter the Ip address either as dotted decimal or prefix\n");
            Console.WriteLine("Use these two templates as a reference:");
            Console.WriteLine("192.128.1.0/255.255.255.0  ||  192.128.1.0/24\n");
            Console.WriteLine("If the ip address is of class D or E, please write the subnet mask as /0\n\n");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Start();

            void Start() {
                Console.Clear();
                ResetVariables();

                //Only if there was an error it will display the error message
                if (errorMsg != "")
                {
                    Console.WriteLine(errorMsg + "\n");
                    errorMsg = "";
                }
                Console.Write("Insert Ip Address: ");
                //Gets the Ip address to be manipulated
                string userInput = Console.ReadLine();

                MakeCalculations();

                Console.Clear();

                //Shows the information to the user
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                Console.WriteLine("Ip Address (Decimal): " + ipAddr);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                Console.WriteLine("Subnet Mask (Decimal): " + subMaskStr);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                Console.WriteLine("Network Address (Decimal): " + networkAddr);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                Console.WriteLine("Ip Address (Binary): " + ipAddrBin);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                Console.WriteLine("Subnet Mask (Binary): " + subMaskBin);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                Console.WriteLine("Network Address (Binary): " + networkAddrBin);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                Console.WriteLine("Class: " + ipClass);
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                if (hosts > 0)
                {
                    Console.WriteLine("No of Hosts: " + hosts);
                    Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                    Console.WriteLine("No of Usable Hosts: " + (hosts-2));
                    Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                    Console.WriteLine("No of subnets: " + subnetsNo);
                }
                else if (hosts == -1)
                {
                    Console.WriteLine("Hosts: No hosts available for this ip class");
                    Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                    Console.WriteLine("No of Subnets: No subnets available for this ip class");
                }
                else
                {
                    Console.WriteLine("Hosts: Error! Calculating hosts failed");
                    Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                    Console.WriteLine("No of Subnets: Error! Calculating subnets failed");
                }

                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 1);
                Console.Write("Press any key to continue...");

                Console.ReadKey();
                Start();


                void MakeCalculations()
                {
                    //This means that the user placed the slash in a wrong place or has an invalid ip address
                    if (userInput.IndexOf("/") < 7 || userInput.IndexOf("/") > 15)
                    {
                        errorMsg = "Slash was placed in an invalid spot or the ip address might be invalid";
                        Start();
                    }
                    else
                    {
                        inputExploded = userInput.Split('/');

                        //this means that the user used more than one slash
                        if (inputExploded.Length > 2)
                        {
                            errorMsg = "Please use only one slash placed between the ip address and the subnet mask";
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
                                subnetExists = true;

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
                                subnetExists = true;

                                //Assigns the subnet array to each individual
                                //value of the exploded string and convert them to integers
                                subnetExploded = subMaskRaw.Split('.');
                                subnetInt = strArrToIntArr(subnetExploded);

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
                            //This means that the user is entering an address of
                            //class D or E, which do not have subnets by default
                            else if (subMaskRaw.Length == 1 && Convert.ToInt32(subMaskRaw) == 0)
                            {
                                subnetExists = false;
                                subMaskStr = "No Subnet for this Ip Class";
                                subMaskBin = "No Subnet for this Ip Class";
                            }
                            //This means that the user entered no subnet
                            else
                            {
                                errorMsg = "Please enter a subnet mask";
                                Start();
                            }

                            //Gets an array of the ip sections in decimal and one in binary
                            ipExplodedInt = strArrToIntArr(ipExploded);
                            ipBin = intArrToBinArr(ipExplodedInt);

                            //Beautifies the binary ip address that's going to be shown to the user
                            foreach (string ipSec in ipBin)
                            {
                                ipAddrBin += ipSec + '.';
                            }
                            ipAddrBin = ipAddrBin.TrimEnd('.');

                            //Is used later to calculate the amount of subnets
                            int startingIndex = 0;

                            //Gets the class of the ip address
                            //Also, if the subnet mask isn't right for a given class, it will send back an error
                            if (subnetExists)
                            {
                                if (ipExplodedInt[0] >= 1 && ipExplodedInt[0] <= 127)
                                {
                                    ipClass = 'A';
                                    startingIndex = 1;
                                    if (subnetInt[0] < 255) { errorMsg = "The default subnet mask for this class is 255.0.0.0"; Start(); }
                                }
                                else if (ipExplodedInt[0] >= 128 && ipExplodedInt[0] <= 191)
                                {
                                    ipClass = 'B';
                                    startingIndex = 2;
                                    if (subnetInt[0] < 255 || subnetInt[1] < 255) { errorMsg = "The default subnet mask for this class is 255.255.0.0"; Start(); }
                                }
                                else if (ipExplodedInt[0] >= 192 && ipExplodedInt[0] <= 223)
                                {
                                    ipClass = 'C';
                                    startingIndex = 3;
                                    if (subnetInt[0] < 255 || subnetInt[1] < 255 || subnetInt[2] < 255) { errorMsg = "The default subnet mask for this class is 255.255.255.0"; Start(); }
                                }
                                else
                                {
                                    errorMsg = "Subnet mask error";
                                    Start();
                                }
                            }
                            //Ip classes D and E should not have a subnet
                            else if (!subnetExists)
                            {
                                startingIndex = -1;
                                if (ipExplodedInt[0] >= 224 && ipExplodedInt[0] <= 239)
                                {
                                    ipClass = 'D';
                                }
                                else if (ipExplodedInt[0] >= 240 && ipExplodedInt[0] <= 255)
                                {
                                    ipClass = 'E';
                                }
                                else
                                {
                                    Start();
                                }
                            }
                            else
                            {
                                errorMsg = "Subnet mask error";
                                Start();
                            }

                            //Network address (binary)
                            string[] binAddr = new string[4];
                            if (ipClass == 'D' || ipClass == 'E')
                            {
                                binAddr = ipBin;
                            }
                            else
                            {
                                int p = 0;
                                foreach (string sec in ipBin)
                                {
                                    int n = 0;
                                    foreach (char bit in sec)
                                    {
                                        if (ipBin[p][n] == '1' && subnetBin[p][n] == '1')
                                        {
                                            binAddr[p] += ipBin[p][n];
                                        }
                                        else
                                        {
                                            binAddr[p] += '0';
                                        }
                                        n++;
                                    }
                                    p++;
                                }
                            }
                            //Output to the user
                            foreach (string sec in binAddr)
                            {
                                networkAddrBin += sec;
                                networkAddrBin += '.';
                            }
                            networkAddrBin = networkAddrBin.TrimEnd('.');

                            //Network address (Dotted Decimal)
                            networkAddr = BinArrToDottedDec(binAddr);

                            //Hosts
                            if (ipClass == 'D' || ipClass == 'E')
                            {
                                hosts = -1;
                            }
                            else
                            {
                                int hostBits = 0;

                                //Gets the amount of host bits in a subnet mask
                                foreach (string sec in subnetBin)
                                {
                                    foreach (char bit in sec)
                                    {
                                        if (bit == '0') { hostBits++; }
                                    }
                                }
                                //The actual equasion to getting the hosts --> h = 2^n
                                hosts = Convert.ToInt32(Math.Pow(2, hostBits));
                            }

                            //Subnets
                            //Also tests for a wrong subnet section value
                            if (ipClass == 'D' || ipClass == 'E')
                            {
                                hosts = -1;
                            }
                            else
                            {
                                int subnetBits = 0;
                                bool zeroFound = false;

                                for (int i = startingIndex; i < 4; i++)
                                {
                                    foreach (char bit in subnetBin[i])
                                    {

                                        if(zeroFound == true)
                                        {
                                            if (bit == '1')
                                            {
                                                errorMsg = "Subnet mask values should be one of the following: 128, 224, 240, 248, 252, 254, 255. Anything other than that is classified as an invalid subnet mask";
                                                Start();
                                            }
                                        }
                                        else
                                        {
                                            if (bit == '1') { subnetBits++; }
                                            else if (bit == '0') { zeroFound = true; }
                                        }
                                    }
                                }
                                subnetsNo = Convert.ToInt32(Math.Pow(2, subnetBits));
                            }
                        }
                    }
                }
            }

            //Tests an Ip addr / sub mask for inconsistencies and returns an array with the values of each section
            int[] strArrToIntArr(string[] inputArr)
            {
                int[] inputExplodedInt = new int[4];

                //This means the user hasn't punctuated the address correctly
                if (inputArr.Length > 4 || inputArr.Length < 4)
                {
                    errorMsg = "Puctuation error; Make sure to puctuate the ip address/subnet mask correctly";
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
                            errorMsg = "Not a number or invalid range of numbers; Make sure each section of the ip address/subnet mask is a number between 0 - 255";
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

            //Converts a binary string into a dotted decimal
            string BinArrToDottedDec(string[] binVal)
            {
                string output = "";
                int[] dottedSecInt = new int[4];

                //Converts each section to an integer
                int k = 0;
                foreach (string binSec in binVal)
                {
                    int bitValue = 128;
                    int finalInt = 0;
                    foreach (char bit in binSec)
                    {
                        if (bit == '1') { finalInt += bitValue; }
                        bitValue /= 2;
                    }
                    dottedSecInt[k] = finalInt;
                    k++;
                }

                //Joins all sections into one string and adds points to separate each section
                for (int i = 0; i < 4; i++)
                {
                    output += Convert.ToString(dottedSecInt[i]) + '.';
                }
                output = output.TrimEnd('.');
                return output;
            }

            void ResetVariables()
            {
                ipAddr = "";
                ipAddrBin = "";
                subMaskStr = "";
                subMaskBin = "";
                networkAddr = "";
                networkAddrBin = "";
                subnetExists = true;
            }
        }
    }
}
