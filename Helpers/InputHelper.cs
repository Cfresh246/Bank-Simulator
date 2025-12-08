using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_Simulator.Output;

namespace Bank_Simulator.Input
{
    public static class InputHelper
    {
        public static int GetInt(string message, int min, int max)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int value) || (value < min || value > max))
                {
                    OutputHelpers.ColorError();
                    Console.WriteLine($"Please enter a number between {min} and {max}.");
                    continue;
                }
                return value;
            }
        }
        public static decimal GetDec(string message)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (!decimal.TryParse(input, out decimal value))
                {
                    Console.WriteLine("Invalid amount");
                    continue;
                }
                return value;
            }
        }
        public static string GetString(string message, string messageOutput)
        {
            while(true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    OutputHelpers.ColorError();
                    Console.WriteLine(messageOutput);
                    continue;
                }
                return input;
            }
        }
        public static bool StringLengthVerification(string input, int length, string errorMessage)
        {
            if (input.Length != 4 || !int.TryParse(input, out int value)) 
            {
                OutputHelpers.ColorError();
                Console.WriteLine(errorMessage);
                return false;
            }
            return true;
        }
        public static string GetAccountNumber()
        {
            string accountNumber = "";

            Random generator = new Random();
            accountNumber += generator.Next(1, 10);

            for (int i = 0; i < 8;  i++)
            {
                accountNumber += generator.Next(0, 10);
            }
            return accountNumber;
        }
    }
}
