﻿using System;
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
            string subMask = "";

            //Introduction
            Console.WriteLine("Welcome to the Ip Calculator 2000");
            Console.WriteLine("With this tool you can insert any ip address and get back the network address and the number of usable hosts");
            Console.WriteLine("Please enter the Ip address either as dotted decimal or prefix");
            Console.WriteLine("Use these two templates as a reference:");
            Console.WriteLine("192.128.1.0/255.255.255.0  ||  192.128.1.0/24\n\n");
            Start();

            void Start() {
                Console.Write("Insert IP Address: ");
                //Gets the Ip address to be manipulated
                string userInput = Console.ReadLine();

                MakeCalculations();

                Console.Clear();
                Console.SetCursorPosition(Console.CursorLeft + 10, Console.CursorTop + 3);
                Console.WriteLine(ipAddr);


                Console.ReadKey();


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

                            //This means the user hasn't punctuated the Ip address correctly
                            if (ipExploded.Length - 1 > 3 || ipExploded.Length - 1 < 3)
                            {
                                Start();
                            }
                            else
                            {
                                foreach (string ip in ipExploded)
                                {

                                }
                                
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
        }
    }
}
