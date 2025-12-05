using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Simulator.Output
{
    public static class OutputHelpers
    {
        public static void KeyContinue()
        {
            Console.Write("Press any key to continue..."); Console.ReadKey();
        }
        public static void ColorError()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Error: ");
            Console.ResetColor();
        }
    }
}
