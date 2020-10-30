﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTerminal
{
    class CnslFormatter
    {
        // Prompts user for an input, with the message parameter serving as context. Returns the string generated by the user's input.
        // Does not allow blank inputs, and will repeat until an input is given.
        public static string PromptForInput(string message)
        {
            while (true)
            {
                Console.Write(message);
                string userInput = (Console.ReadLine()).Trim();
                if (userInput.Length > 0)
                {
                    return userInput;
                }
            }
        }

        // Prompts user to answer a yes or no question. 
        // If yes, return true. If no, return false. 
        // Other answers are considered invalid, and the method will loop until a valid answer is given.
        public static bool AskYesOrNo(string message)
        {
            while (true)
            {
                string promptMsg = message + " (y/n) ";
                string inputStr = PromptForInput(promptMsg);
                inputStr = inputStr.Trim().ToLower();
                if (inputStr.Equals("y"))
                {
                    return true;
                }
                else if (inputStr.Equals("n"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine($"Error: Please input y/Y or n/N.");
                }
            }
        }

        // Stops program until user inputs a keystroke
        public static void PauseByAnyKey()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        // Adds x empty lines in console for formatting
        public static void MakeLineSpace(int x)
        {
            for (int i = 0; i < x; i++)
            {
                Console.WriteLine(" ");
            }
        }
    }
}
