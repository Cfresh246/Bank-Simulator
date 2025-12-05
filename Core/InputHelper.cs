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
        public static int GetInt(string message, int max, int min)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int value) || (value < min || value > max))
                {
                    OutputHelpers.ColorError();
                    Console.WriteLine("Please enter a number between {min} and {max}.");
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
        public static int GetString(string message)
        {
            while(true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    OutputHelpers.ColorError();
                }
            }
        }
    }
}
